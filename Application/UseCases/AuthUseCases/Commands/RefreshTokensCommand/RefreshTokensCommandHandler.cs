using Application.Common.Configuration;
using Application.Common.OutputModels.IdentityOutputModels;
using Application.Common.Providers;
using Application.Common.Services;
using Application.Common.Transactions;
using Application.Common.Utils;
using Application.Repositories.IdentityRepositories;
using MediatR;
using Microsoft.Extensions.Options;
using Utils.Exceptions.CustomExceptions;

namespace Application.UseCases.AuthUseCases.Commands.RefreshTokensCommand;

public class RefreshTokensCommandHandler(
    IOptions<TokenLifetimeSettings> tokenLifetimeSettings,
    IClaimRepository claimRepository,
    IUserRepository userRepository, 
    IAuthenticationService authenticationService,
    ITokenService tokenService,
    IDateTimeProvider dateTimeProvider,
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
                throw AuthorizationFailedException.DueToInvalidRefreshToken();
            
            // Checking refresh token
            if (user.RefreshTokenExpirationTime == null || user.RefreshTokenExpirationTime < DateTime.UtcNow)
                throw AuthorizationFailedException.DueToInvalidRefreshToken(user.Id);
            
            // Querying and filling claims
            var claims = await claimRepository.GetByRoleIdAsync(user.RoleId, cancellationToken);
            var filledClaims = ClaimHelper.FillMissingClaimsWithLowestPermission(user.Role, claims);
            
            // Rotating tokens
            var newAccessToken = tokenService.CreateAccessToken(user.ClinicId, user.Id, filledClaims);
            var newRefreshToken = tokenService.CreateRefreshToken();
            
            // Updating user
            var refreshTokenHash = authenticationService.HashToken(newRefreshToken);
            user.UpdateRefreshTokenHashAndExpirationTime(
                refreshTokenHash, 
                dateTimeProvider.UtcNow.AddDays(tokenLifetimeSettings.Value.RefreshTokenLifetimeInDays),
                dateTimeProvider.UtcNow);
            
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