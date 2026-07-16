using Application.Common.Behaviours.Validation.Rules;
using FluentValidation;

namespace Application.UseCases.PatientUseCases.Commands.UnarchivePatientCommand;

public class UnarchivePatientCommandValidator : AbstractValidator<UnarchivePatientCommand>
{
    public UnarchivePatientCommandValidator()
    {
        RuleFor(command => command.Id).ValidRequiredGuid();
    }
}