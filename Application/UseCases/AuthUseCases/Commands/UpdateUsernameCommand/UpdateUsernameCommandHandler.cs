using Application.Common.Exceptions;
using Application.Common.Logging;
using Application.Common.OutputModels.IdentityOutputModels;
using Application.Common.Transactions;
using Application.Repositories.ClinicianRepositories;
using Application.Repositories.IdentityRepositories;
using Domain.Commons.Utils.Helper;
using Domain.Entities.ClinicianEntities;
using Domain.Entities.IdentityEntities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.AuthUseCases.Commands.UpdateUsernameCommand;

public class UpdateUsernameCommandHandler(
    ILogger<UpdateUsernameCommand> logger,
    IClinicianRepository clinicianRepository,
    IRoleRepository roleRepository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) 
    : IRequestHandler<UpdateUsernameCommand, UpdateUsernameOutputModel>
{
    public Task<UpdateUsernameOutputModel> Handle(UpdateUsernameCommand request, CancellationToken cancellationToken)
    {
        return unitOfWork.ExecuteAsync(async () =>
        {
            // Checking old username for correctness
            var normalizedOldUsername = StringHelper.Normalize(request.OldUsername);
            if (normalizedOldUsername != request.User.NormalizedUsername)
            {
                logger.LogWarning(LogMessages.EntityPropertyInvalid, nameof(request.OldUsername), nameof(User));
                throw new InvalidPropertyValueException(nameof(User), nameof(User.Username));
            }
            
            // Checking usernames for equality
            var normalizedNewUsername = StringHelper.Normalize(request.NewUsername);
            if (normalizedNewUsername == normalizedOldUsername)
            {
                logger.LogWarning(LogMessages.EntityPropertyUnchanged, nameof(request.NewUsername), nameof(User));
                throw new PropertyUnchangedException(nameof(User), nameof(User.Username));
            }
            
            // Checking new username for uniqueness
            var existingUsername = await userRepository.GetByClinicIdAndNormalizedUsernameAsync(
                request.Clinic.Id, 
                normalizedNewUsername, 
                cancellationToken);
            if (existingUsername != null)
            {
                logger.LogWarning(LogMessages.EntityPropertyAlreadyInUse, nameof(request.NewUsername), nameof(User));
                throw new PropertyAlreadyInUseException<string>(nameof(User), nameof(User.Username), request.NewUsername);
            }
            
            // Updating username
            request.User.UpdateUsername(request.NewUsername);
            
            // Querying and checking role
            var role = await roleRepository.GetByClinicIdAndRoleIdAsync(
                request.Clinic.Id, 
                request.User.RoleId, 
                cancellationToken);
            if (role == null)
            {
                logger.LogWarning(LogMessages.EntityNotFound, nameof(role), request.User.RoleId);
                throw new NotFoundException(nameof(Role), request.User.RoleId);
            }
            
            // Querying and checking clinician
            Clinician? clinician = null;
            if (request.User.ClinicianId is { } clinicianId)
            {
                clinician = await clinicianRepository.GetByClinicIdAndClinicianIdAsync(
                    request.Clinic.Id, 
                    clinicianId, 
                    cancellationToken);
                if (clinician == null)
                {
                    logger.LogWarning(LogMessages.EntityNotFound, nameof(clinician), clinicianId);
                    throw new NotFoundException(nameof(Clinician), clinicianId);
                }
            }
            
            // Returning output model
            return new UpdateUsernameOutputModel(request.User, role, clinician);
        }, cancellationToken);
    }
}