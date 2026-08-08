using Application.Common.Behaviours.ValidationBehaviour.Rules;
using FluentValidation;

namespace Application.UseCases.DeviceCategoryUseCases.Queries.ReadDeviceCategoryQuery;

public class ReadDeviceCategoryQueryValidator : AbstractValidator<ReadDeviceCategoryQuery>
{
    public ReadDeviceCategoryQueryValidator()
    {
        RuleFor(query => query.Id).ValidRequiredGuid();
    }
}