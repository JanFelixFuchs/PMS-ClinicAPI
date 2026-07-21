using Application.Common.Behaviours.Validation.Rules;
using FluentValidation;

namespace Application.UseCases.RoleUseCases.Commands.DeleteRoleCommand;

public class DeleteRoleCommandValidator : AbstractValidator<DeleteRoleCommand>
{
    public DeleteRoleCommandValidator()
    {
        RuleFor(command => command.Id).ValidRequiredGuid();
    }
}