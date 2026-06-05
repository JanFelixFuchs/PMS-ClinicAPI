using FluentValidation;

namespace Application.Common.Behaviours.Validation.Rules;

public static class EnumValidationRules
{
    public static IRuleBuilderOptions<T, TProperty> ValidRequiredEnum<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder) 
        where TProperty : struct, Enum =>
        ruleBuilder
            .IsInEnum()
            .WithMessage("{PropertyName} must be a valid option");
}