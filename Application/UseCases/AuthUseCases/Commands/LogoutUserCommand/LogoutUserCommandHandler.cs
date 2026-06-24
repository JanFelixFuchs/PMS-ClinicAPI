using Application.Common.Exceptions;
using Application.Common.Logging;
using Application.Common.Transactions;
using Application.Repositories.IdentityRepositories;
using Domain.Entities.IdentityEntities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.AuthUseCases.Commands.LogoutUserCommand;

public class LogoutUserCommandHandler(
    ILogger<LogoutUserCommandHandler> logger,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<LogoutUserCommand>
{
    public async Task Handle(LogoutUserCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.ExecuteAsync(async () =>
        {
            // Querying and checking user
            var user = await userRepository.GetByClinicIdAndUserIdAsync(
                request.Clinic.Id, 
                request.User.Id, 
                cancellationToken);
            if (user == null)
            {
                logger.LogWarning(LogMessages.EntityNotFound, nameof(user), request.User);
                throw new NotFoundException(nameof(User), request.User.Id);
            }
            
            // Updating user
            user.MarkRefreshTokenHashAsExpired();
        }, cancellationToken);
    }
}