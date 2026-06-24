using Application.Common.Behaviours.Validation.Rules;
using Domain.Commons.Utils.Constants;
using FluentValidation;

namespace Application.UseCases.AppointmentUseCases.Commands.UpdateAppointmentCommand;

public class UpdateAppointmentCommandValidator : AbstractValidator<UpdateAppointmentCommand>
{
    public UpdateAppointmentCommandValidator()
    {
        RuleFor(command => command.Id).ValidRequiredGuid();

        RuleFor(command => command.Title)
            .ValidRequiredString()
            .ValidRequiredMaximumStringLength(Lengths.AppointmentTitle);
        
        RuleFor(command => command.StartTime)
            .ValidRequiredDateTime()
            .ValidRequiredFutureDateTime()
            .ValidRequiredBeforeDateTime(command => command.EndTime);
            
        RuleFor(command => command.EndTime)
            .ValidRequiredDateTime()
            .ValidRequiredFutureDateTime()
            .ValidRequiredAfterDateTime(command => command.StartTime);
        
        RuleFor(command => command.AppointmentCategoryIds)
            .ValidRequiredCollection()
            .ValidRequiredDuplicateFreeCollection();
        RuleForEach(command => command.AppointmentCategoryIds).ValidRequiredGuid();
        
        RuleFor(command => command.PatientId).ValidRequiredGuid();
        
        RuleFor(command => command.RoomId).ValidRequiredGuid();
        
        RuleFor(command => command.DeviceIds)
            .ValidRequiredCollection()
            .ValidRequiredDuplicateFreeCollection();
        RuleForEach(command => command.DeviceIds).ValidRequiredGuid();
        
        RuleFor(command => command.ClinicianIds)
            .ValidRequiredCollection()
            .ValidRequiredDuplicateFreeCollection()
            .ValidRequiredMinimumCollectionLength();
        RuleForEach(command => command.ClinicianIds).ValidRequiredGuid();
    }
}