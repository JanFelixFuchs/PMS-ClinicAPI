using Application.Common.Behaviours.Validation.Rules;
using FluentValidation;

namespace Application.UseCases.ClinicianCategoryUseCases.Queries.ReadClinicianCategoryQuery;

public class ReadClinicianCategoryQueryValidator : AbstractValidator<ReadClinicianCategoryQuery>
{
    public ReadClinicianCategoryQueryValidator()
    {
        RuleFor(query => query.Id).ValidRequiredGuid();
    }
}