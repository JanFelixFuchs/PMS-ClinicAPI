using Application.Common.Behaviours.ValidationBehaviour.Rules;
using FluentValidation;

namespace Application.UseCases.UserUseCases.Commands.UnarchiveUserCommand;

public class UnarchiveUserCommandValidator : AbstractValidator<UnarchiveUserCommand>
{
    public UnarchiveUserCommandValidator()
    {
        RuleFor(command => command.Id).ValidRequiredGuid();
    }
}