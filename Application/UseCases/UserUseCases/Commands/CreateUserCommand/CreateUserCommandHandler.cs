using Application.Common.Exceptions;
using Application.Common.OutputModels.IdentityOutputModels;
using Application.Common.Providers;
using Application.Common.Services;
using Application.Common.Transactions;
using Application.Repositories.ClinicianRepositories;
using Application.Repositories.IdentityRepositories;
using Domain.Common.Utils.Helper;
using Domain.Entities.ClinicianEntities;
using Domain.Entities.IdentityEntities;
using MediatR;

namespace Application.UseCases.UserUseCases.Commands.CreateUserCommand;

public class CreateUserCommandHandler(
    IClinicianRepository clinicianRepository,
    IRoleRepository roleRepository,
    IUserRepository userRepository,
    IAuthenticationService authenticationService,
    IDateTimeProvider dateTimeProvider,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateUserCommand, UserDetailedOutputModel>
{
    public async Task<UserDetailedOutputModel> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        return await unitOfWork.ExecuteAsync(async () =>
        {
            // Checking username for uniqueness
            var normalizedUsername = StringHelper.Normalize(request.Username);
            var existingUser = await userRepository.GetByClinicIdAndNormalizedUsernameAsync(
                request.Clinic.Id, 
                normalizedUsername, 
                cancellationToken);
            if (existingUser != null)
                throw new PropertyValueAlreadyInUseException<string>(nameof(User), nameof(User.Username), request.Username);
            
            // Querying and checking role
            var role = await roleRepository.GetByClinicIdAndRoleIdAsync(
                request.Clinic.Id, 
                request.RoleId, 
                cancellationToken);
            if (role == null)
                throw new NotFoundException(nameof(Role), request.RoleId);
            
            // Querying and checking clinician for existence 
            var clinician = await clinicianRepository.GetByClinicIdAndClinicianIdAsync(
                request.Clinic.Id, 
                request.ClinicianId, 
                cancellationToken,
                clinician => clinician.User);
            if (clinician == null)
                throw new NotFoundException(nameof(Clinician), request.ClinicianId);
            
            // Checking clinician for use by another user
            if (clinician.User != null)
                throw new PropertyValueAlreadyInUseException<Guid>(nameof(Clinician), nameof(Clinician.Id), request.ClinicianId);
            
            // Validating and hashing password
            var passwordHash = authenticationService.ValidateAndHashPassword(request.Password);
            
            // Creating user
            var user = new User(
                request.Clinic, 
                request.Username, 
                passwordHash, 
                false, 
                role, 
                clinician,
                dateTimeProvider.UtcNow);
            
            // Adding user
            await userRepository.AddAsync(user, cancellationToken);
            
            // Returning output model
            return new UserDetailedOutputModel(user, role, clinician);
        }, cancellationToken);
    }
}