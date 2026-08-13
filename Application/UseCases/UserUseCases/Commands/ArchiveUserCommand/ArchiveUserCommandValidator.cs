using Application.Common.Behaviours.ValidationBehaviour.Rules;
using FluentValidation;

namespace Application.UseCases.UserUseCases.Commands.ArchiveUserCommand;

public class ArchiveUserCommandValidator : AbstractValidator<ArchiveUserCommand>
{
    public ArchiveUserCommandValidator()
    {
        RuleFor(command => command.Id).ValidRequiredGuid();
    }
}