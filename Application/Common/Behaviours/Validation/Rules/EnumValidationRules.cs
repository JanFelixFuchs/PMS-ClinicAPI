using FluentValidation;
using Utils.Exceptions.Errors.Codes;

namespace Application.Common.Behaviours.Validation.Rules;

public static class EnumValidationRules
{
    public static IRuleBuilderOptions<T, TProperty> ValidRequiredEnum<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder) 
        where TProperty : struct, Enum =>
        ruleBuilder
            .IsInEnum()
            .WithState(_ => ErrorCode.INVALID_ENUM_VALUE)
            .WithMessage("{PropertyName} must be a valid option");
}