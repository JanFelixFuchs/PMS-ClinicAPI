using Application.Common.Behaviours.Validation.Rules;
using FluentValidation;

namespace Application.UseCases.DeviceUseCases.Commands.DeleteDeviceCommand;

public class DeleteDeviceCommandValidator : AbstractValidator<DeleteDeviceCommand>
{
    public DeleteDeviceCommandValidator()
    {
        RuleFor(command => command.Id).ValidRequiredGuid();
    }
}