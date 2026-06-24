using Application.Common.Behaviours.Validation.Rules;
using FluentValidation;

namespace Application.UseCases.AppointmentUseCases.Commands.MarkAppointmentAsAttendedCommand;

public class MarkAppointmentAsAttendedCommandValidator : AbstractValidator<MarkAppointmentAsAttendedCommand>
{
    public MarkAppointmentAsAttendedCommandValidator()
    {
        RuleFor(command => command.Id).ValidRequiredGuid();
    }
}