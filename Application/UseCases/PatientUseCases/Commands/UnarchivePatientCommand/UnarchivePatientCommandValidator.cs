using Application.Common.Behaviours.ValidationBehaviour.Rules;
using FluentValidation;

namespace Application.UseCases.PatientUseCases.Commands.UnarchivePatientCommand;

public class UnarchivePatientCommandValidator : AbstractValidator<UnarchivePatientCommand>
{
    public UnarchivePatientCommandValidator()
    {
        RuleFor(command => command.Id).ValidRequiredGuid();
    }
}