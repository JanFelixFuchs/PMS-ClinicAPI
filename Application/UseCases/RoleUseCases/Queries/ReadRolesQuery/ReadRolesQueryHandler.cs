using Application.Common.OutputModels.IdentityOutputModels;
using Application.Repositories.IdentityRepositories;
using MediatR;

namespace Application.UseCases.RoleUseCases.Queries.ReadRolesQuery;

public class ReadRolesQueryHandler(
    IRoleRepository roleRepository) 
    : IRequestHandler<ReadRolesQuery, List<RoleOverviewOutputModel>>
{
    public async Task<List<RoleOverviewOutputModel>> Handle(ReadRolesQuery request, CancellationToken cancellationToken)
    {
        // Querying roles
        var roles = await roleRepository.GetByClinicIdAsync(
            request.Clinic.Id,
            cancellationToken);
        
        // Returning output model
        return roles
            .Select(role => new RoleOverviewOutputModel(role))
            .ToList();
    }
}