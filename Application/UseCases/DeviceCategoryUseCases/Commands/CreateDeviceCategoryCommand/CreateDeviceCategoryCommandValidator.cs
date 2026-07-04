using Application.Common.Behaviours.Validation.Rules;
using Domain.Commons.Utils.Constants;
using FluentValidation;

namespace Application.UseCases.DeviceCategoryUseCases.Commands.CreateDeviceCategoryCommand;

public class CreateDeviceCategoryCommandValidator : AbstractValidator<CreateDeviceCategoryCommand>
{
    public CreateDeviceCategoryCommandValidator()
    {
        RuleFor(command => command.Name)
            .ValidRequiredString()
            .ValidRequiredMaximumStringLength(Lengths.CategoryName);
        
        RuleFor(command => command.Abbreviation)
            .ValidRequiredString()
            .ValidRequiredMaximumStringLength(Lengths.Abbreviation);
        
        RuleFor(command => command.Color)
            .ValidRequiredString()
            .ValidRequiredRegex(RegexPatterns.Color);
        
        RuleFor(command => command.DeviceIds)
            .ValidRequiredCollection()
            .ValidRequiredDuplicateFreeCollection();
        RuleForEach(command => command.DeviceIds).ValidRequiredGuid();
    }
}