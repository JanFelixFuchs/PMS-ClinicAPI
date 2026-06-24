using Application.Common.Behaviours.Validation.Rules;
using FluentValidation;

namespace Application.UseCases.AppointmentUseCases.Queries.ReadAppointmentsQuery;

public class ReadAppointmentsQueryValidator : AbstractValidator<ReadAppointmentsQuery>
{
    public ReadAppointmentsQueryValidator()
    {
        RuleFor(query => query.StartDate)
            .ValidRequiredDate()
            .ValidRequiredBeforeDate(query => query.EndDate);
        
        RuleFor(query => query.EndDate)
            .ValidRequiredDate()
            .ValidRequiredAfterDate(query => query.StartDate);
    }
}