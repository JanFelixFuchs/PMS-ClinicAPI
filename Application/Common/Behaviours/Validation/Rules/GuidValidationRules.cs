using FluentValidation;
using Utils.Exceptions.Errors.Codes;

namespace Application.Common.Behaviours.Validation.Rules;

public static class GuidValidationRules
{
    public static IRuleBuilderOptions<T, Guid> ValidRequiredGuid<T>(this IRuleBuilder<T, Guid> ruleBuilder) =>
        ruleBuilder
            .NotEmpty()
            .WithState(_ => ErrorCode.MISSING_VALUE)
            .WithMessage("{PropertyName} must be not empty");
    
    public static IRuleBuilderOptions<T, Guid?> ValidOptionalGuid<T>(this IRuleBuilder<T, Guid?> ruleBuilder) =>
        ruleBuilder
            .Must(value => value == null || value != Guid.Empty)
            .WithState(_ => ErrorCode.EMPTY_VALUE)
            .WithMessage("{PropertyName} must be null or not empty");
}