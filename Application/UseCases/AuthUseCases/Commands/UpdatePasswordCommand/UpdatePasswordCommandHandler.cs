using Application.Common.Configuration;
using Application.Common.Exceptions;
using Application.Common.Logging;
using Application.Common.OutputModels.IdentityOutputModels;
using Application.Common.Services;
using Application.Common.Transactions;
using Application.Common.Utils;
using Application.Repositories.IdentityRepositories;
using Domain.Entities.IdentityEntities;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Application.UseCases.AuthUseCases.Commands.UpdatePasswordCommand;

public class UpdatePasswordCommandHandler(
    ILogger<UpdatePasswordCommandHandler> logger,
    IOptions<TokenLifetimeSettings> tokenLifetimeSettings,
    IClaimRepository claimRepository,
    IRoleRepository roleRepository,
    IAuthenticationService authenticationService,
    ITokenService tokenService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdatePasswordCommand, (UpdatePasswordOutputModel Payload, string RefreshToken)> 
{
    public Task<(UpdatePasswordOutputModel Payload, string RefreshToken)> Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
    {
        return unitOfWork.ExecuteAsync(async () =>
        {
            // Checking old password for correctness
            var oldPasswordIsCorrect = authenticationService.CheckPassword(request.User.PasswordHash, request.OldPassword);
            if (!oldPasswordIsCorrect)
            {
                logger.LogWarning(LogMessages.EntityPropertyInvalid, nameof(request.OldPassword), nameof(User));
                throw new InvalidPropertyValueException(nameof(User), nameof(User.PasswordHash));
            }
            
            // Checking passwords for equality
            var passwordsAreEqual = authenticationService.CheckPassword(request.User.PasswordHash, request.NewPassword);
            if (passwordsAreEqual)
            {
                logger.LogWarning(LogMessages.EntityPropertyUnchanged, nameof(request.NewPassword), nameof(User));
                throw new PropertyUnchangedException(nameof(User), nameof(User.PasswordHash));
            }
            
            // Validating and hashing new password
            var newPasswordHash = authenticationService.ValidateAndHashPassword(request.NewPassword);
            
            // Querying role
            var role = await roleRepository.GetByClinicIdAndRoleIdAsync(
                request.Clinic.Id, 
                request.User.RoleId, 
                cancellationToken);
            if (role == null)
            {
                logger.LogWarning(LogMessages.EntityNotFound, nameof(role), request.User.RoleId);
                throw new NotFoundException(nameof(Role), request.User.RoleId);
            }
            
            // Querying and filling claims
            var claims = await claimRepository.GetByRoleIdAsync(role.Id, cancellationToken);
            var filledClaims = ClaimHelper.FillMissingClaimsWithLowestPermission(role, claims);
            
            // Creating tokens
            var accessToken = tokenService.CreateAccessToken(request.Clinic.Id, request.User.Id, filledClaims);
            var refreshToken = tokenService.CreateRefreshToken();
            
            // Updating user
            var refreshTokenHash = authenticationService.HashToken(refreshToken);
            request.User.UpdateRefreshTokenHashAndExpirationTime(refreshTokenHash, DateTime.UtcNow.AddDays(tokenLifetimeSettings.Value.RefreshTokenLifetimeInDays));
            request.User.UpdatePasswordHash(newPasswordHash);
            
            // Returning output model and refresh token as tuple
            var payload = new UpdatePasswordOutputModel(accessToken);
            return (payload, refreshToken);
        }, cancellationToken);
    }
}