using Application.Common.Exceptions;
using Application.Common.OutputModels.IdentityOutputModels;
using Application.Common.Providers;
using Application.Common.Transactions;
using Application.Repositories.IdentityRepositories;
using Domain.Entities.IdentityEntities;
using MediatR;

namespace Application.UseCases.UserUseCases.Commands.ArchiveUserCommand;

public class ArchiveUserCommandHandler(
    IUserRepository userRepository,
    IDateTimeProvider dateTimeProvider,
    IUnitOfWork unitOfWork)
    : IRequestHandler<ArchiveUserCommand, UserDetailedOutputModel>
{
    public async Task<UserDetailedOutputModel> Handle(ArchiveUserCommand request, CancellationToken cancellationToken)
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
            
            // Archiving user
            user.Archive(dateTimeProvider.UtcNow);
            
            // Returning output model
            return new UserDetailedOutputModel(user, user.Role, user.Clinician);
        }, cancellationToken);
    }
}