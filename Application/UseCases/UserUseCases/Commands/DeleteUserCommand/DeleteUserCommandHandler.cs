using Application.Common.Exceptions;
using Application.Common.Transactions;
using Application.Repositories.IdentityRepositories;
using Domain.Entities.IdentityEntities;
using MediatR;

namespace Application.UseCases.UserUseCases.Commands.DeleteUserCommand;

public class DeleteUserCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) 
    : IRequestHandler<DeleteUserCommand>
{
    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.ExecuteAsync(async () =>
        {
            // Querying and checking user
            var user = await userRepository.GetByClinicIdAndUserIdAsync(
                request.Clinic.Id, 
                request.Id,
                cancellationToken);
            if (user == null)
                throw new NotFoundException(nameof(User), request.Id);
            
            // Deleting user
            user.Delete();
        }, cancellationToken);
    }
}