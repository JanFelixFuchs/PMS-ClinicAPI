using Application.Common.Configuration;
using Application.Common.Exceptions;
using Application.Common.Logging;
using Application.Common.OutputModels.IdentityOutputModels;
using Application.Common.Services;
using Application.Common.Transactions;
using Application.Common.Utils;
using Application.Repositories.IdentityRepositories;
using Domain.Commons.Utils.Helper;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Application.UseCases.AuthUseCases.Commands.LoginUserCommand;

public class LoginUserCommandHandler(
    ILogger<LoginUserCommandHandler> logger,
    IOptions<TokenLifetimeSettings> tokenLifetimeSettings,
    IClaimRepository claimRepository,
    IClinicRepository clinicRepository,
    IUserRepository userRepository,
    IAuthenticationService authenticationService,
    ITokenService tokenService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<LoginUserCommand, (LoginUserOutputModel Payload, string RefreshToken)>
{
    public async Task<(LoginUserOutputModel Payload, string RefreshToken)> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        return await unitOfWork.ExecuteAsync(async () =>
        {
            // Querying and checking clinic
            var normalizedCode = StringHelper.Normalize(request.Code);
            var clinic = await clinicRepository.GetByNormalizedCodeAsync(normalizedCode, cancellationToken);
            if (clinic == null)
            {
                logger.LogWarning(LogMessages.EntityNotFound, nameof(clinic), request.Code);
                throw new AuthorizationFailedException();
            }
            
            // Querying and checking user
            var normalizedUsername = StringHelper.Normalize(request.Username);
            var user = await userRepository.GetByClinicIdAndNormalizedUsernameAsync(
                clinic.Id, 
                normalizedUsername,
                cancellationToken,
                user => user.Role,
                user => user.Clinician);
            if (user == null || user.IsDeleted || user.IsArchived)
            {
                logger.LogWarning(LogMessages.EntityNotFound, nameof(user), request.Username);
                throw new AuthorizationFailedException();
            }
            
            // Checking password
            var passwordIsCorrect = authenticationService.CheckPassword(user.PasswordHash, request.Password);
            if (!passwordIsCorrect)
            {
                logger.LogWarning(LogMessages.InvalidPassword, user.Id);
                throw new AuthorizationFailedException();
            }
            
            // Querying and filling claims
            var claims = await claimRepository.GetByRoleIdAsync(user.RoleId, cancellationToken);
            var filledClaims = ClaimHelper.FillMissingClaimsWithLowestPermission(user.Role, claims);
            
            // Creating tokens
            var accessToken = tokenService.CreateAccessToken(user.ClinicId, user.Id, filledClaims);
            var refreshToken = tokenService.CreateRefreshToken();
            
            // Updating user
            var refreshTokenHash = authenticationService.HashToken(refreshToken);
            user.UpdateRefreshTokenHashAndExpirationTime(refreshTokenHash, DateTime.UtcNow.AddDays(tokenLifetimeSettings.Value.RefreshTokenLifetimeInDays));
            
            // Returning output model and refresh token as tuple
            var payload = new LoginUserOutputModel(clinic, user, user.Role, user.Clinician, accessToken);
            return (payload, refreshToken);
        }, cancellationToken);
    }
}