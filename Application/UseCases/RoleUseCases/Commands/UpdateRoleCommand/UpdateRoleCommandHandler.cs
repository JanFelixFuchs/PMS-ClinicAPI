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

namespace Application.UseCases.RoleUseCases.Commands.UpdateRoleCommand;

public class UpdateRoleCommandHandler(
    ILogger<UpdateRoleCommandHandler> logger,
    IClaimRepository claimRepository,
    IRoleRepository roleRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateRoleCommand, RoleDetailedOutputModel>
{
    public async Task<RoleDetailedOutputModel> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        return await unitOfWork.ExecuteAsync(async () =>
        {
            // Querying and checking role
            var role = await roleRepository.GetByClinicIdAndRoleIdAsync(
                request.Clinic.Id, 
                request.Id,
                cancellationToken,
                role => role.Users);
            if (role == null)
            {
                logger.LogWarning(LogMessages.EntityNotFound, nameof(role), request.Id);
                throw new NotFoundException(nameof(Role), request.Id);
            }
            
            // Checking role name for uniqueness
            var normalizedRoleName = StringHelper.Normalize(request.Name);
            if (normalizedRoleName != role.NormalizedName)
            {
                var existingRole = await roleRepository.GetByClinicIdAndNormalizedNameAsync(
                    request.Clinic.Id, 
                    normalizedRoleName, 
                    cancellationToken);
                if (existingRole != null)
                {
                    logger.LogWarning(LogMessages.EntityPropertyAlreadyInUse, nameof(request.Name), nameof(Role));
                    throw new PropertyAlreadyInUseException<string>(nameof(Role), nameof(Role.Name), request.Name);
                }
            }
            
            // Updating role
            role.Update(request.Name);
            
            // Creating claims
            var claims = ClaimHelper.CreateClaimsFromDictionary(role, request.Claims);
            
            // Deleting existing claims
            await claimRepository.DeleteByRoleIdAsync(request.Id, cancellationToken);
            
            // Adding new claims
            await claimRepository.AddExceptValueEqualsNoneAsync(claims, cancellationToken);
            
            // Returning output model
            return new RoleDetailedOutputModel(role, role.Users, claims);
        }, cancellationToken);
    }
}