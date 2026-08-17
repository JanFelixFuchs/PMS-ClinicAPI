using Application.Common.Exceptions;
using Application.Common.OutputModels.IdentityOutputModels;
using Application.Common.Transactions;
using Application.Repositories.IdentityRepositories;
using Domain.Entities.IdentityEntities;
using MediatR;

namespace Application.UseCases.UserUseCases.Commands.UpdateUserCommand;

public class UpdateUserCommandHandler(
    IRoleRepository roleRepository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) 
    : IRequestHandler<UpdateUserCommand, UserDetailedOutputModel>
{
    public async Task<UserDetailedOutputModel> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
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
            
            // Querying and checking role
            var role = await roleRepository.GetByClinicIdAndRoleIdAsync(
                request.Clinic.Id, 
                request.RoleId, 
                cancellationToken);
            if (role == null)
                throw new NotFoundException(nameof(Role), request.RoleId);
            
            // Updating user
            user.UpdateRole(role);
            
            // Returning output model
            return new UserDetailedOutputModel(user, role, user.Clinician);
        }, cancellationToken);
    }
}