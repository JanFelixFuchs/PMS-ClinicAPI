using Application.Common.Behaviours.Validation.Rules;
using FluentValidation;

namespace Application.UseCases.PatientUseCases.Commands.DeletePatientCommand;

public class DeletePatientCommandValidator : AbstractValidator<DeletePatientCommand>
{
    public DeletePatientCommandValidator()
    {
        RuleFor(command => command.Id).ValidRequiredGuid();
    }
}