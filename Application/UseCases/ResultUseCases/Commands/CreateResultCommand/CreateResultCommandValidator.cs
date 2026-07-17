using Application.Common.Behaviours.Validation.Rules;
using Domain.Commons.Utils.Constants;
using FluentValidation;

namespace Application.UseCases.ResultUseCases.Commands.CreateResultCommand;

public class CreateResultCommandValidator : AbstractValidator<CreateResultCommand>
{
    public CreateResultCommandValidator()
    {
        RuleFor(command => command.Title)
            .ValidRequiredString()
            .ValidRequiredMaximumStringLength(Lengths.ResultTitle);
        
        RuleFor(command => command.DateOfCreation)
            .ValidRequiredDateTime()
            .ValidRequiredPastDateTime();
        
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