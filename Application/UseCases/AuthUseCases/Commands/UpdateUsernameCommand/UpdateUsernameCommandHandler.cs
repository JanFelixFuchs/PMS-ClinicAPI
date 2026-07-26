using Application.Common.Exceptions;
using Application.Common.OutputModels.IdentityOutputModels;
using Application.Common.Transactions;
using Application.Repositories.ClinicianRepositories;
using Application.Repositories.IdentityRepositories;
using Domain.Commons.Utils.Helper;
using Domain.Entities.ClinicianEntities;
using Domain.Entities.IdentityEntities;
using MediatR;

namespace Application.UseCases.AuthUseCases.Commands.UpdateUsernameCommand;

public class UpdateUsernameCommandHandler(
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
                throw new IncorrectPropertyValueException(nameof(User), nameof(User.Username));
            
            // Checking usernames for equality
            var normalizedNewUsername = StringHelper.Normalize(request.NewUsername);
            if (normalizedNewUsername == normalizedOldUsername)
                throw new UnchangedPropertyValueException(nameof(User), nameof(User.Username));
            
            // Checking new username for uniqueness
            var existingUsername = await userRepository.GetByClinicIdAndNormalizedUsernameAsync(
                request.Clinic.Id, 
                normalizedNewUsername, 
                cancellationToken);
            if (existingUsername != null)
                throw new PropertyValueAlreadyInUseException<string>(nameof(User), nameof(User.Username), request.NewUsername);
            
            // Updating username
            request.User.UpdateUsername(request.NewUsername);
            
            // Querying and checking role
            var role = await roleRepository.GetByClinicIdAndRoleIdAsync(
                request.Clinic.Id, 
                request.User.RoleId, 
                cancellationToken);
            if (role == null)
                throw new NotFoundException(nameof(Role), request.User.RoleId);
            
            // Querying and checking clinician
            Clinician? clinician = null;
            if (request.User.ClinicianId is { } clinicianId)
            {
                clinician = await clinicianRepository.GetByClinicIdAndClinicianIdAsync(
                    request.Clinic.Id, 
                    clinicianId, 
                    cancellationToken);
                if (clinician == null)
                    throw new NotFoundException(nameof(Clinician), clinicianId);
            }
            
            // Returning output model
            return new UpdateUsernameOutputModel(request.User, role, clinician);
        }, cancellationToken);
    }
}