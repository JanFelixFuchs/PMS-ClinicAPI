using Application.Common.Behaviours.ValidationBehaviour.Rules;
using FluentValidation;

namespace Application.UseCases.ClinicianUseCases.Commands.DeleteClinicianCommand;

public class DeleteClinicianCommandValidator : AbstractValidator<DeleteClinicianCommand>
{
    public DeleteClinicianCommandValidator()
    {
        RuleFor(command => command.Id).ValidRequiredGuid();
    }
}