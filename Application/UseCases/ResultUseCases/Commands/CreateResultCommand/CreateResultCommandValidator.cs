using Application.Common.Behaviours.ValidationBehaviour.Rules;
using Application.Common.Providers;
using Domain.Common.Utils.Constants;
using FluentValidation;

namespace Application.UseCases.ResultUseCases.Commands.CreateResultCommand;

public class CreateResultCommandValidator : AbstractValidator<CreateResultCommand>
{
    public CreateResultCommandValidator(IDateTimeProvider dateTimeProvider)
    {
        var currentDateTime = dateTimeProvider.UtcNow;
        
        RuleFor(command => command.Title)
            .ValidRequiredString()
            .ValidRequiredMaximumStringLength(Lengths.ResultTitle);
        
        RuleFor(command => command.DateOfCreation)
            .ValidRequiredDateTime()
            .ValidRequiredPastDateTime(currentDateTime);
        
        RuleFor(command => command.Appendix)
            .ValidRequiredArray()
            .ValidRequiredMinimumArrayLength()
            .ValidRequiredMaximumArrayLength(Lengths.Appendix);
        
        RuleFor(command => command.Remarks)
            .ValidOptionalString()
            .ValidOptionalMaximumStringLength(Lengths.ResultRemarks);
        
        RuleFor(command => command.PatientId).ValidRequiredGuid();
        
        RuleFor(command => command.ClinicianId).ValidRequiredGuid();

        RuleFor(command => command.DeviceId).ValidOptionalGuid();
    }
}