using FluentValidation;

namespace Application.Common.Behaviours.Validation.Rules;

public static class GuidValidationRules
{
    public static IRuleBuilderOptions<T, Guid> ValidRequiredGuid<T>(this IRuleBuilder<T, Guid> ruleBuilder) =>
        ruleBuilder
            .NotEmpty()
            .WithMessage("{PropertyName} must be not empty");
    
    public static IRuleBuilderOptions<T, Guid?> ValidOptionalGuid<T>(this IRuleBuilder<T, Guid?> ruleBuilder) =>
        ruleBuilder
            .Must(value => value == null || value != Guid.Empty)
            .WithMessage("{PropertyName} must be null or not empty");
}