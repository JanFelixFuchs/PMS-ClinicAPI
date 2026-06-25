using Application.Common.Configuration;
using Application.Common.Exceptions;
using Application.Common.Logging;
using Application.Common.OutputModels.IdentityOutputModels;
using Application.Common.Services;
using Application.Common.Transactions;
using Application.Common.Utils;
using Application.Repositories.IdentityRepositories;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Application.UseCases.AuthUseCases.Commands.RefreshTokensCommand;

public class RefreshTokensCommandHandler(
    ILogger<RefreshTokensCommandHandler> logger,
    IOptions<TokenLifetimeSettings> tokenLifetimeSettings,
    IClaimRepository claimRepository,
    IUserRepository userRepository, 
    IAuthenticationService authenticationService,
    ITokenService tokenService,
    IUnitOfWork unitOfWork) 
    : IRequestHandler<RefreshTokensCommand, (RefreshTokensOutputModel Payload, string RefreshToken)>
{
    public async Task<(RefreshTokensOutputModel Payload, string RefreshToken)> Handle(RefreshTokensCommand request, CancellationToken cancellationToken)
    {
        return await unitOfWork.ExecuteAsync(async () =>
        {
            // Hashing refresh token
            var currentRefreshTokenHash = authenticationService.HashToken(request.RefreshToken);
            
            // Querying user and checking user
            var user = await userRepository.GetByRefreshTokenHashAsync(
                currentRefreshTokenHash, 
                cancellationToken,
                user => user.Clinic,
                user => user.Role,
                user => user.Clinician);
            if (user == null || user.IsDeleted || user.IsArchived)
            {
                logger.LogWarning(LogMessages.EntityNotFound, nameof(user), user?.Id);
                throw new AuthorizationFailedException();
            }
            
            // Checking refresh token
            if (user.RefreshTokenExpirationTime == null || user.RefreshTokenExpirationTime < DateTime.UtcNow)
            {
                logger.LogWarning(LogMessages.InvalidRefreshToken, user.Id);
                throw new AuthorizationFailedException();
            }
            
            // Querying and filling claims
            var claims = await claimRepository.GetByRoleIdAsync(user.RoleId, cancellationToken);
            var filledClaims = ClaimHelper.FillMissingClaimsWithLowestPermission(user.Role, claims);
            
            // Rotating tokens
            var newAccessToken = tokenService.CreateAccessToken(user.ClinicId, user.Id, filledClaims);
            var newRefreshToken = tokenService.CreateRefreshToken();
            
            // Updating user
            var refreshTokenHash = authenticationService.HashToken(newRefreshToken);
            user.UpdateRefreshTokenHashAndExpirationTime(refreshTokenHash, DateTime.UtcNow.AddDays(tokenLifetimeSettings.Value.RefreshTokenLifetimeInDays));
            
            // Returning output model and refresh token as tuple
            var payload = new RefreshTokensOutputModel(
                user.Clinic,
                user,
                user.Role,
                user.Clinician,
                newAccessToken);
            return (payload, newRefreshToken);
        }, cancellationToken);
    }
}