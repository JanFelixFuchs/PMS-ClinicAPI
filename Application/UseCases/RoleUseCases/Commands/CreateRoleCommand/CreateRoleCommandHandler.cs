using Application.Common.Exceptions;
using Application.Common.Logging;
using Application.Common.OutputModels.IdentityOutputModels;
using Application.Common.Transactions;
using Application.Common.Utils;
using Application.Repositories.IdentityRepositories;
using Domain.Commons.Utils.Helper;
using Domain.Entities.IdentityEntities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.RoleUseCases.Commands.CreateRoleCommand;

public class CreateRoleCommandHandler(
    ILogger<CreateRoleCommandHandler> logger,
    IClaimRepository claimRepository,
    IRoleRepository roleRepository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) 
    : IRequestHandler<CreateRoleCommand, RoleDetailedOutputModel>
{
    public async Task<RoleDetailedOutputModel> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        return await unitOfWork.ExecuteAsync(async () =>
        {
            // Checking role name for uniqueness
            var normalizedRoleName = StringHelper.Normalize(request.Name);
            var existingRole = await roleRepository.GetByClinicIdAndNormalizedNameAsync(
                request.Clinic.Id, 
                normalizedRoleName, 
                cancellationToken);
            if (existingRole != null)
            {
                logger.LogWarning(LogMessages.EntityPropertyAlreadyInUse, nameof(request.Name), nameof(Role));
                throw new PropertyAlreadyInUseException<string>(nameof(Role), nameof(Role.Name), request.Name);
            }
            
            // Querying and checking users
            var users = await userRepository.GetByClinicIdAndUserIdsAsync(
                request.Clinic.Id, 
                request.UserIds, 
                cancellationToken,
                user => user.Role);
            var missingUserIds = request.UserIds.Except(users.Select(user => user.Id)).ToList();
            if (missingUserIds.Count > 0)
            {
                logger.LogWarning(LogMessages.EntitiesNotFound, nameof(users), missingUserIds);
                throw new NotFoundException(nameof(User), missingUserIds);
            }
            
            // Creating role
            var role = new Role(request.Clinic, request.Name,false);
            
            // Creating claims
            var claims = ClaimHelper.CreateClaimsFromDictionary(role, request.Claims);
            
            // Adding role
            await roleRepository.AddAsync(role, cancellationToken);
            
            // Adding claims
            await claimRepository.AddExceptValueEqualsNoneAsync(claims, cancellationToken);
            
            // Updating user roles
            foreach (var user in users)
                user.UpdateRole(role);
            
            // Returning output model
            return new RoleDetailedOutputModel(role, users, claims);
        }, cancellationToken);
    }
}