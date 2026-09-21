using Application.Common.Behaviours.ValidationBehaviour.Rules;
using Application.Common.Providers;
using Domain.Common.Utils.Constants;
using FluentValidation;

namespace Application.UseCases.AppointmentUseCases.Commands.CreateAppointmentCommand;

public class CreateAppointmentCommandValidator : AbstractValidator<CreateAppointmentCommand>
{
    public CreateAppointmentCommandValidator(IDateTimeProvider dateTimeProvider)
    {
        var currentDateTime = dateTimeProvider.UtcNow;
        
        RuleFor(command => command.Title)
            .ValidRequiredString()
            .ValidRequiredMaximumStringLength(Lengths.AppointmentTitle);
        
        RuleFor(command => command.StartTime)
            .ValidRequiredDateTime()
            .ValidRequiredFutureDateTime(currentDateTime)
            .ValidRequiredBeforeDateTime(command => command.EndTime);
            
        RuleFor(command => command.EndTime)
            .ValidRequiredDateTime()
            .ValidRequiredFutureDateTime(currentDateTime)
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