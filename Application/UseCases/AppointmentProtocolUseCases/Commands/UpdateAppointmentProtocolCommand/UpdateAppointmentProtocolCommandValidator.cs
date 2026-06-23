using Application.Common.Behaviours.Validation.Rules;
using Domain.Commons.Utils.Constants;
using FluentValidation;

namespace Application.UseCases.AppointmentProtocolUseCases.Commands.UpdateAppointmentProtocolCommand;

public class UpdateAppointmentProtocolCommandValidator : AbstractValidator<UpdateAppointmentProtocolCommand>
{
    public UpdateAppointmentProtocolCommandValidator()
    {
        RuleFor(command => command.Id).ValidRequiredGuid();
        
        RuleFor(command => command.Symptoms)
            .ValidOptionalString()
            .ValidOptionalMaximumStringLength(Lengths.Symptoms);
        
        RuleFor(command => command.Diagnosis)
            .ValidOptionalString()
            .ValidOptionalMaximumStringLength(Lengths.Diagnosis);

        RuleFor(command => command.Treatment)
            .ValidOptionalString()
            .ValidOptionalMaximumStringLength(Lengths.Treatment);
        
        RuleFor(command => command.Remarks)
            .ValidOptionalString()
            .ValidOptionalMaximumStringLength(Lengths.AppointmentProtocolRemarks);

        RuleFor(command => command.ClinicianId).ValidRequiredGuid();
        
        RuleFor(command => command.RoomId).ValidRequiredGuid();

        RuleFor(command => command.DeviceIds)
            .ValidRequiredCollection()
            .ValidRequiredDuplicateFreeCollection();
        RuleForEach(command => command.DeviceIds).ValidRequiredGuid();
    }
}