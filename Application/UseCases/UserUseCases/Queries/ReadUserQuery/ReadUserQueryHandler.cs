using Application.Common.Exceptions;
using Application.Common.OutputModels.IdentityOutputModels;
using Application.Repositories.IdentityRepositories;
using Domain.Entities.IdentityEntities;
using MediatR;

namespace Application.UseCases.UserUseCases.Queries.ReadUserQuery;

public class ReadUserQueryHandler(IUserRepository userRepository) 
    : IRequestHandler<ReadUserQuery, UserDetailedOutputModel>
{
    public async Task<UserDetailedOutputModel> Handle(ReadUserQuery request, CancellationToken cancellationToken)
    {
        // Querying and checking user
        var user = await userRepository.GetByClinicIdAndUserIdAsync(
            request.Clinic.Id, 
            request.Id, 
            cancellationToken,
            user => user.Role,
            user => user.Clinician);
        if (user == null)
            throw new NotFoundException(nameof(User), request.Id);
        
        // Returning output model
        return new UserDetailedOutputModel(user, user.Role, user.Clinician);
    }
}