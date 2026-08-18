using Application.Common.OutputModels.IdentityOutputModels;
using Application.Repositories.IdentityRepositories;
using MediatR;

namespace Application.UseCases.UserUseCases.Queries.ReadUsersQuery;

public class ReadUsersQueryHandler(
    IUserRepository userRepository) 
    : IRequestHandler<ReadUsersQuery, List<UserOverviewOutputModel>>
{
    public async Task<List<UserOverviewOutputModel>> Handle(ReadUsersQuery request, CancellationToken cancellationToken)
    {
        // Querying users
        var users = await userRepository.GetByClinicIdAsync(
            request.Clinic.Id,
            request.Archived,
            cancellationToken);
        
        // Returning output model
        return users
            .Select(user => new UserOverviewOutputModel(user))
            .ToList();
    }
}