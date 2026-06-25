using Application.Common.Behaviours.Validation.Rules;
using Domain.Commons.Utils.Constants;
using FluentValidation;

namespace Application.UseCases.AuthUseCases.Commands.RegisterClinicCommand;

public class RegisterClinicCommandValidator : AbstractValidator<RegisterClinicCommand>
{
    public RegisterClinicCommandValidator()
    {
        RuleFor(command => command.Code)
            .ValidRequiredString()
            .ValidRequiredRegex(RegexPatterns.Code);

        RuleFor(command => command.Name)
            .ValidRequiredString()
            .ValidRequiredMaximumStringLength(Lengths.ClinicName);
        
        RuleFor(command => command.Abbreviation)
            .ValidRequiredString()
            .ValidRequiredMaximumStringLength(Lengths.Abbreviation);
        
        RuleFor(command => command.Owner).
            ValidRequiredString()
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
        
        RuleFor(command => command.Username)
            .ValidRequiredString()
            .ValidRequiredRegex(RegexPatterns.Username);
        
        RuleFor(command => command.Password)
            .ValidRequiredString()
            .ValidRequiredRegex(RegexPatterns.Password);

        RuleFor(command => command.RoleNameWithNoRights)
            .ValidRequiredString()
            .ValidRequiredMaximumStringLength(Lengths.RoleName);
        
        RuleFor(command => command.RoleNameWithAllRights)
            .ValidRequiredString()
            .ValidRequiredMaximumStringLength(Lengths.RoleName);
    }
}