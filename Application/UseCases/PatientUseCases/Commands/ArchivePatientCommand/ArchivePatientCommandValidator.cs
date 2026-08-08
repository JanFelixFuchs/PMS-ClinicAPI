using Application.Common.Behaviours.ValidationBehaviour.Rules;
using FluentValidation;

namespace Application.UseCases.PatientUseCases.Commands.ArchivePatientCommand;

public class ArchivePatientCommandValidator : AbstractValidator<ArchivePatientCommand>
{
    public ArchivePatientCommandValidator()
    {
        RuleFor(command => command.Id).ValidRequiredGuid();
    }
}