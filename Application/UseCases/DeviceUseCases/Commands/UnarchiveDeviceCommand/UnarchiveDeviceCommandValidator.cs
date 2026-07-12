using Application.Common.Behaviours.Validation.Rules;
using FluentValidation;

namespace Application.UseCases.DeviceUseCases.Commands.UnarchiveDeviceCommand;

public class UnarchiveDeviceCommandValidator : AbstractValidator<UnarchiveDeviceCommand>
{
    public UnarchiveDeviceCommandValidator()
    {
        RuleFor(command => command.Id).ValidRequiredGuid();
    }
}