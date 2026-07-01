using Application.Common.Behaviours.Validation.Rules;
using Domain.Commons.Utils.Constants;
using FluentValidation;

namespace Application.UseCases.ClinicUseCases.Commands.UpdateClinicCommand;

public class UpdateClinicCommandValidator : AbstractValidator<UpdateClinicCommand>
{
    public UpdateClinicCommandValidator()
    {
        RuleFor(command => command.Name)
            .ValidRequiredString()
            .ValidRequiredMaximumStringLength(Lengths.ClinicName);
        
        RuleFor(command => command.Abbreviation)
            .ValidRequiredString()
            .ValidRequiredMaximumStringLength(Lengths.Abbreviation);
        
        RuleFor(command => command.Owner)
            .ValidRequiredString()
            .ValidRequiredMaximumStringLength(Lengths.Owner);
        
        RuleFor(command => command.MedicalField).ValidRequiredEnum();
        
        RuleFor(command => command.Country).ValidRequiredEnum();
        
        RuleFor(command => command.Street)
            .ValidRequiredString()
            .ValidRequiredMaximumStringLength(Lengths.Street);

        RuleFor(command => command.HouseNumber)
            .ValidRequiredString()
            .ValidRequiredMaximumStringLength(Lengths.HouseNumber);
        
        RuleFor(command => command.City)
            .ValidRequiredString()
            .ValidRequiredMaximumStringLength(Lengths.City);
        
        RuleFor(command => command.ZipCode)
            .ValidRequiredString()
            .ValidRequiredRegex(command => RegexPatterns.GetZipCodeRegexPattern(command.Country));
        
        RuleFor(command => command.Email)
            .ValidRequiredString()
            .ValidRequiredRegex(RegexPatterns.Email);
        
        RuleFor(command => command.PhoneNumber)
            .ValidRequiredString()
            .ValidRequiredRegex(command => RegexPatterns.GetPhoneNumberRegexPattern(command.Country));
    }
}