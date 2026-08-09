using Application.Common.Exceptions;
using Application.Common.Providers;
using Application.Common.Transactions;
using Application.Repositories.IdentityRepositories;
using Domain.Entities.IdentityEntities;
using MediatR;

namespace Application.UseCases.AuthUseCases.Commands.LogoutUserCommand;

public class LogoutUserCommandHandler(
    IUserRepository userRepository,
    IDateTimeProvider dateTimeProvider,
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
                throw new NotFoundException(nameof(User), request.User.Id);
            
            // Updating user
            user.MarkRefreshTokenHashAsExpired(dateTimeProvider.UtcNow);
        }, cancellationToken);
    }
}