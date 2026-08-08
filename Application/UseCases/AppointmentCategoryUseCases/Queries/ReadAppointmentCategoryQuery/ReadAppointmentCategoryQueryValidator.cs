using Application.Common.Behaviours.ValidationBehaviour.Rules;
using FluentValidation;

namespace Application.UseCases.AppointmentCategoryUseCases.Queries.ReadAppointmentCategoryQuery;

public class ReadAppointmentCategoryQueryValidator : AbstractValidator<ReadAppointmentCategoryQuery>
{
    public ReadAppointmentCategoryQueryValidator()
    {
        RuleFor(query => query.Id).ValidRequiredGuid();
    }
}