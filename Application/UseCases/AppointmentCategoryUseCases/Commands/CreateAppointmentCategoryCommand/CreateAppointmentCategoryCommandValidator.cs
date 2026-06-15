using Application.Common.Behaviours.Validation.Rules;
using Domain.Commons.Utils.Constants;
using FluentValidation;

namespace Application.UseCases.AppointmentCategoryUseCases.Commands.CreateAppointmentCategoryCommand;

public class CreateAppointmentCategoryCommandValidator : AbstractValidator<CreateAppointmentCategoryCommand>
{
    public CreateAppointmentCategoryCommandValidator()
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
    }
}