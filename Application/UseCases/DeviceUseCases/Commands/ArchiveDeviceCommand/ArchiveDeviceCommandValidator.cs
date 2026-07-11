using Application.Common.Behaviours.Validation.Rules;
using FluentValidation;

namespace Application.UseCases.DeviceUseCases.Commands.ArchiveDeviceCommand;

public class ArchiveDeviceCommandValidator : AbstractValidator<ArchiveDeviceCommand>
{
    public ArchiveDeviceCommandValidator()
    {
        RuleFor(command => command.Id).ValidRequiredGuid();
    }
}