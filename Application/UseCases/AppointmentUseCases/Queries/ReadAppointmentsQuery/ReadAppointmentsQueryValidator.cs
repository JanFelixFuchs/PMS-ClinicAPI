using Application.Common.Behaviours.Validation.Rules;
using FluentValidation;

namespace Application.UseCases.AppointmentUseCases.Queries.ReadAppointmentsQuery;

public class ReadAppointmentsQueryValidator : AbstractValidator<ReadAppointmentsQuery>
{
    public ReadAppointmentsQueryValidator()
    {
        RuleFor(query => query.StartDateTime)
            .ValidRequiredDateTime()
            .ValidRequiredBeforeDateTime(query => query.EndDateTime);
        
        RuleFor(query => query.EndDateTime)
            .ValidRequiredDateTime()
            .ValidRequiredAfterDateTime(query => query.StartDateTime);
    }
}