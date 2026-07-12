using Application.Common.Behaviours.Validation.Rules;
using FluentValidation;

namespace Application.UseCases.DeviceUseCases.Commands.UpdateDeviceStatusCommand;

public class UpdateDeviceStatusCommandValidator : AbstractValidator<UpdateDeviceStatusCommand>
{
    public UpdateDeviceStatusCommandValidator()
    {
        RuleFor(command => command.Id).ValidRequiredGuid();
        
        RuleFor(command => command.Status).ValidRequiredEnum();
    }
}