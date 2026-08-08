using Application.Common.Behaviours.ValidationBehaviour.Rules;
using FluentValidation;

namespace Application.UseCases.AppointmentProtocolUseCases.Commands.StartAppointmentProtocolCommand;

public class StartAppointmentProtocolCommandValidator : AbstractValidator<StartAppointmentProtocolCommand>
{
    public StartAppointmentProtocolCommandValidator()
    {
        RuleFor(command => command.Id).ValidRequiredGuid();
    }
}