using Application.Common.Behaviours.ValidationBehaviour.Rules;
using FluentValidation;

namespace Application.UseCases.AppointmentProtocolUseCases.Queries.ReadAppointmentProtocolQuery;

public class ReadAppointmentProtocolQueryValidator : AbstractValidator<ReadAppointmentProtocolQuery>
{
    public ReadAppointmentProtocolQueryValidator()
    {
        RuleFor(query => query.Id).ValidRequiredGuid();
    }
}