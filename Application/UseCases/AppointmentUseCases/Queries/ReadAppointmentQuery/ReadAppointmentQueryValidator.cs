using Application.Common.Behaviours.ValidationBehaviour.Rules;
using FluentValidation;

namespace Application.UseCases.AppointmentUseCases.Queries.ReadAppointmentQuery;

public class ReadAppointmentQueryValidator : AbstractValidator<ReadAppointmentQuery>
{
    public ReadAppointmentQueryValidator()
    {
        RuleFor(query => query.Id).ValidRequiredGuid();
    }
}