using Application.Common.Behaviours.Validation.Rules;
using FluentValidation;

namespace Application.UseCases.DeviceCategoryUseCases.Queries.ReadDeviceCategoryQuery;

public class ReadDeviceCategoryQueryValidator : AbstractValidator<ReadDeviceCategoryQuery>
{
    public ReadDeviceCategoryQueryValidator()
    {
        RuleFor(query => query.Id).ValidRequiredGuid();
    }
}