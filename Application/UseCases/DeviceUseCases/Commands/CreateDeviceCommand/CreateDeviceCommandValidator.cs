using Application.Common.Behaviours.ValidationBehaviour.Rules;
using Application.Common.Providers;
using Domain.Common.Utils.Constants;
using FluentValidation;

namespace Application.UseCases.DeviceUseCases.Commands.CreateDeviceCommand;

public class CreateDeviceCommandValidator : AbstractValidator<CreateDeviceCommand>
{
    public CreateDeviceCommandValidator(IDateTimeProvider dateTimeProvider)
    {
        var currentDateTime = dateTimeProvider.UtcNow; 
        
        RuleFor(command => command.Name)
            .ValidRequiredString()
            .ValidRequiredMaximumStringLength(Lengths.DeviceName);
        
        RuleFor(command => command.Abbreviation)
            .ValidRequiredString()
            .ValidRequiredMaximumStringLength(Lengths.Abbreviation);

        RuleFor(command => command.SerialNumber)
            .ValidRequiredString()
            .ValidRequiredMaximumStringLength(Lengths.SerialNumber);
        
        RuleFor(command => command.Status).ValidRequiredEnum();
        
        RuleFor(command => command.Producer)
            .ValidRequiredString()
            .ValidRequiredMaximumStringLength(Lengths.Producer);
        
        RuleFor(command => command.DeviceCategoryIds)
            .ValidRequiredCollection()
            .ValidRequiredDuplicateFreeCollection();
        RuleForEach(command => command.DeviceCategoryIds).ValidRequiredGuid();
        
        RuleFor(command => command.DateOfPurchase)
            .ValidOptionalDateTime()
            .ValidOptionalPastDateTime(currentDateTime);
            
        RuleFor(command => command.DateOfLastMaintenance)
            .ValidOptionalDateTime()
            .ValidOptionalPastDateTime(currentDateTime);
    }
}