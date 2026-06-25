using Application.Common.Configuration;
using Application.Common.Exceptions;
using Application.Common.Logging;
using Application.Common.OutputModels.IdentityOutputModels;
using Application.Common.Services;
using Application.Common.Transactions;
using Application.Common.Utils;
using Application.Repositories.IdentityRepositories;
using Domain.Commons.Utils.Helper;
using Domain.Entities.IdentityEntities;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Application.UseCases.AuthUseCases.Commands.RegisterClinicCommand;

public class RegisterClinicCommandHandler(
    ILogger<RegisterClinicCommandHandler> logger,
    IOptions<TokenLifetimeSettings> tokenLifetimeSettings,
    IClaimRepository claimRepository,
    IClinicRepository clinicRepository,
    IRoleRepository roleRepository,
    IUserRepository userRepository,
    IAuthenticationService authenticationService,
    ITokenService tokenService,
    IUnitOfWork unitOfWork) 
    : IRequestHandler<RegisterClinicCommand, (RegisterClinicOutputModel Payload, string RefreshToken)>
{
    public async Task<(RegisterClinicOutputModel Payload, string RefreshToken)> Handle(RegisterClinicCommand request, CancellationToken cancellationToken)
    {
        return await unitOfWork.ExecuteAsync(async () =>
        {
            // Checking code for uniqueness
            var normalizedCode = StringHelper.Normalize(request.Code);
            var existingClinic = await clinicRepository.GetByNormalizedCodeAsync(normalizedCode, cancellationToken);
            if (existingClinic != null)
            {
                logger.LogWarning(LogMessages.EntityPropertyAlreadyInUse, nameof(request.Code), nameof(Clinic));
                throw new PropertyAlreadyInUseException<string>(nameof(Clinic), nameof(Clinic.Code), request.Code);
            }
            
            // Checking role names for uniqueness
            var normalizedRoleNameWithNoRights = StringHelper.Normalize(request.RoleNameWithNoRights);
            var normalizedRoleNameWithAllRights = StringHelper.Normalize(request.RoleNameWithAllRights);
            if (normalizedRoleNameWithNoRights.Equals(normalizedRoleNameWithAllRights))
            {
                logger.LogWarning(LogMessages.EntityPropertyAlreadyInUse, nameof(Role.Name), nameof(Role));
                throw new PropertyAlreadyInUseException<string>(nameof(Role), nameof(Role.Name), request.RoleNameWithAllRights);
            }
            
            // Creating clinic
            var clinic = new Clinic(
                request.Code,
                request.Name,
                request.Abbreviation, 
                request.Owner,
                request.MedicalField,
                request.Street,
                request.HouseNumber,
                request.City,
                request.ZipCode,
                request.Country,
                request.Email,
                request.PhoneNumber);
            
            // Creating default roles
            var roleWithNoRights = new Role(clinic, request.RoleNameWithNoRights, true);
            var roleWithAllRights = new Role(clinic, request.RoleNameWithAllRights, true);
            
            // Creating claims for default role with all rights
            var claimsWithAllRights = ClaimHelper.CreateHighestPermissionClaims(roleWithAllRights);

            // Validating and hashing password
            var passwordHash = authenticationService.ValidateAndHashPassword(request.Password);
            
            // Creating user
            var user = new User(clinic, request.Username, passwordHash, true, roleWithAllRights, null);
            
            // Adding clinic
            await clinicRepository.AddAsync(clinic, cancellationToken);
            
            // Adding roles
            await roleRepository.AddAsync(roleWithNoRights, cancellationToken);
            await roleRepository.AddAsync(roleWithAllRights, cancellationToken);
            
            // Adding claims
            await claimRepository.AddExceptValueEqualsNoneAsync(claimsWithAllRights, cancellationToken);
            
            // Adding user
            await userRepository.AddAsync(user, cancellationToken);
            
            // Creating tokens
            var accessToken = tokenService.CreateAccessToken(clinic.Id, user.Id, claimsWithAllRights);
            var refreshToken = tokenService.CreateRefreshToken();
            
            // Updating user
            var refreshTokenHash = authenticationService.HashToken(refreshToken);
            user.UpdateRefreshTokenHashAndExpirationTime(refreshTokenHash, DateTime.UtcNow.AddDays(tokenLifetimeSettings.Value.RefreshTokenLifetimeInDays));
            
            // Returning payload and refresh token as tuple
            var payload = new RegisterClinicOutputModel(
                clinic,
                user,
                roleWithAllRights,
                null,
                accessToken);
            return (payload, refreshToken);
        }, cancellationToken);
    }
}
