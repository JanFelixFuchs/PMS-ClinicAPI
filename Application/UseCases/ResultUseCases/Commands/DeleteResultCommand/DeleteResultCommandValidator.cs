using Application.Common.Behaviours.Validation.Rules;
using FluentValidation;

namespace Application.UseCases.ResultUseCases.Commands.DeleteResultCommand;

public class DeleteResultCommandValidator : AbstractValidator<DeleteResultCommand>
{
    public DeleteResultCommandValidator()
    {
        RuleFor(command => command.Id).ValidRequiredGuid();
    }
}