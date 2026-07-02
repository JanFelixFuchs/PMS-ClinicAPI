using Application.Common.Behaviours.Validation.Rules;
using FluentValidation;

namespace Application.UseCases.ClinicianUseCases.Commands.DeleteClinicianCommand;

public class DeleteClinicianCommandValidator : AbstractValidator<DeleteClinicianCommand>
{
    public DeleteClinicianCommandValidator()
    {
        RuleFor(command => command.Id).ValidRequiredGuid();
    }
}