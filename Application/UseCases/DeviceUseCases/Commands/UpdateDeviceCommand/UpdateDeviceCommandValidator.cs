using Application.Common.Behaviours.ValidationBehaviour.Rules;
using Application.Common.Providers;
using Domain.Common.Utils.Constants;
using FluentValidation;

namespace Application.UseCases.DeviceUseCases.Commands.UpdateDeviceCommand;

public class UpdateDeviceCommandValidator : AbstractValidator<UpdateDeviceCommand>
{
    public UpdateDeviceCommandValidator(IDateTimeProvider dateTimeProvider)
    {
        var currentDateTime = dateTimeProvider.UtcNow;
        
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
            .ValidOptionalPastDateTime(currentDateTime);
    }
}