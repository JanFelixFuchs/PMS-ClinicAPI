using Application.Common.Exceptions;
using Application.Common.Logging;
using Application.Common.OutputModels.IdentityOutputModels;
using Application.Common.Utils;
using Application.Repositories.IdentityRepositories;
using Domain.Entities.IdentityEntities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.RoleUseCases.Queries.ReadRoleQuery;

public class ReadRoleQueryHandler(
    ILogger<ReadRoleQueryHandler> logger,
    IRoleRepository roleRepository)
    : IRequestHandler<ReadRoleQuery, RoleDetailedOutputModel>
{
    public async Task<RoleDetailedOutputModel> Handle(ReadRoleQuery request, CancellationToken cancellationToken)
    {
        // Querying and checking role
        var role = await roleRepository.GetByClinicIdAndRoleIdAsync(
            request.Clinic.Id,
            request.Id,
            cancellationToken,
            role => role.Users,
            role => role.Claims);
        if (role == null)
        {
            logger.LogWarning(LogMessages.EntityNotFound, nameof(role), request.Id);
            throw new NotFoundException(nameof(Role), request.Id);
        }
        
        // Returning output model
        return new RoleDetailedOutputModel(role, role.Users, ClaimHelper.FillMissingClaimsWithLowestPermission(role, role.Claims));
    }
}