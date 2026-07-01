using Application.Common.Behaviours.Validation.Rules;
using FluentValidation;

namespace Application.UseCases.ClinicianUseCases.Commands.ArchiveClinicianCommand;

public class ArchiveClinicianCommandValidator : AbstractValidator<ArchiveClinicianCommand>
{
    public ArchiveClinicianCommandValidator()
    {
        RuleFor(command => command.Id).ValidRequiredGuid();
    }
}