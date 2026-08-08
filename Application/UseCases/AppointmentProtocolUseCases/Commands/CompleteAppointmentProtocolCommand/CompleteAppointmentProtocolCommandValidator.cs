using Application.Common.Behaviours.ValidationBehaviour.Rules;
using FluentValidation;

namespace Application.UseCases.AppointmentProtocolUseCases.Commands.CompleteAppointmentProtocolCommand;

public class CompleteAppointmentProtocolCommandValidator : AbstractValidator<CompleteAppointmentProtocolCommand>
{
    public CompleteAppointmentProtocolCommandValidator()
    {
        RuleFor(command => command.Id).ValidRequiredGuid();
    }
}