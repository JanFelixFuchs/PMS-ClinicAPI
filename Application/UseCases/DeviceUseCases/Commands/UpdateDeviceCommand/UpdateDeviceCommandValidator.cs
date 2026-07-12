using Application.Common.Behaviours.Validation.Rules;
using Domain.Commons.Utils.Constants;
using FluentValidation;

namespace Application.UseCases.DeviceUseCases.Commands.UpdateDeviceCommand;

public class UpdateDeviceCommandValidator : AbstractValidator<UpdateDeviceCommand>
{
    public UpdateDeviceCommandValidator()
    {
        RuleFor(command => command.Id).ValidRequiredGuid();

        RuleFor(command => command.Name)
            .ValidRequiredString()
            .ValidRequiredMaximumStringLength(Lengths.DeviceName);
        
        RuleFor(command => command.Abbreviation)
            .ValidRequiredString()
            .ValidRequiredMaximumStringLength(Lengths.Abbreviation);
        
        RuleFor(command => command.DeviceCategoryIds)
            .ValidRequiredCollection()
            .ValidRequiredDuplicateFreeCollection();
        RuleForEach(command => command.DeviceCategoryIds).ValidRequiredGuid();
            
        RuleFor(command => command.DateOfLastMaintenance)
            .ValidOptionalDateTime()
            .ValidOptionalPastDateTime();
    }
}