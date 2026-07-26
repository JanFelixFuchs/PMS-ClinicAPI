using Application.Common.Exceptions;
using Application.Common.Transactions;
using Application.Repositories.IdentityRepositories;
using Domain.Entities.IdentityEntities;
using MediatR;

namespace Application.UseCases.RoleUseCases.Commands.DeleteRoleCommand;

public class DeleteRoleCommandHandler(
    IRoleRepository roleRepository,
    IUnitOfWork unitOfWork) 
    : IRequestHandler<DeleteRoleCommand>
{
    public async Task Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.ExecuteAsync(async () =>
        {
            // Querying and checking role
            var role = await roleRepository.GetByClinicIdAndRoleIdAsync(
                request.Clinic.Id, 
                request.Id,
                cancellationToken,
                role => role.Users,
                role => role.Claims);
            if (role == null)
                throw new NotFoundException(nameof(Role), request.Id);
            
            // Deleting role and claims
            role.Delete(role.Users, role.Claims);
        }, cancellationToken);
    }
}