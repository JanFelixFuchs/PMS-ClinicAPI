using Application.Common.Exceptions;
using Application.Common.OutputModels.IdentityOutputModels;
using Application.Common.Transactions;
using Application.Repositories.IdentityRepositories;
using Domain.Entities.IdentityEntities;
using MediatR;

namespace Application.UseCases.UserUseCases.Commands.UnarchiveUserCommand;

public class UnarchiveUserCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UnarchiveUserCommand, UserDetailedOutputModel>
{
    public async Task<UserDetailedOutputModel> Handle(UnarchiveUserCommand request, CancellationToken cancellationToken)
    {
        return await unitOfWork.ExecuteAsync(async () =>
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
            
            // Unarchiving user
            user.Unarchive(user.Clinician);
            
            // Returning output model
            return new UserDetailedOutputModel(user, user.Role, user.Clinician);
        }, cancellationToken);
    }
}