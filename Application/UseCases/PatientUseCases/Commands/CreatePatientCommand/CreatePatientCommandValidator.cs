using Application.Common.Behaviours.Validation.Rules;
using Domain.Commons.Utils.Constants;
using FluentValidation;

namespace Application.UseCases.PatientUseCases.Commands.CreatePatientCommand;

public class CreatePatientCommandValidator : AbstractValidator<CreatePatientCommand>
{
    public CreatePatientCommandValidator()
    {
        RuleFor(command => command.FirstName)
            .ValidRequiredString()
            .ValidRequiredMaximumStringLength(Lengths.FirstName);
        
        RuleFor(command => command.LastName)
            .ValidRequiredString()
            .ValidRequiredMaximumStringLength(Lengths.LastName);
        
        RuleFor(command => command.DateOfBirth)
            .ValidRequiredDateTime()
            .ValidRequiredPastDateTime();
        
        RuleFor(command => command.Gender).ValidRequiredEnum();
        
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
        
        RuleFor(command => command.PhoneNumber).
            ValidRequiredString()
            .ValidRequiredRegex(command => RegexPatterns.GetPhoneNumberRegexPattern(command.Country));
        
        RuleFor(command => command.InsuranceStatus).ValidRequiredEnum();
        
        RuleFor(command => command.Allergies)
            .ValidOptionalString()
            .ValidOptionalMaximumStringLength(Lengths.Allergies);
            
        RuleFor(command => command.Remarks)
            .ValidOptionalString()
            .ValidOptionalMaximumStringLength(Lengths.PatientRemarks);
    }
}