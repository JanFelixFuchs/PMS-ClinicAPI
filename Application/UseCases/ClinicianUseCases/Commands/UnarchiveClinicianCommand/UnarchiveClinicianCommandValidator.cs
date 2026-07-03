using Application.Common.Behaviours.Validation.Rules;
using FluentValidation;

namespace Application.UseCases.ClinicianUseCases.Commands.UnarchiveClinicianCommand;

public class UnarchiveClinicianCommandValidator : AbstractValidator<UnarchiveClinicianCommand>
{
    public UnarchiveClinicianCommandValidator()
    {
        RuleFor(command => command.Id).ValidRequiredGuid();
    }
}