using FluentValidation;
using Utils.Exceptions.Errors.Codes;

namespace Application.Common.Behaviours.ValidationBehaviour.Rules;

public static class StringValidationRules
{
    public static IRuleBuilderOptions<T, string> ValidRequiredString<T>(this IRuleBuilder<T, string> ruleBuilder) =>
        ruleBuilder
            .Must(value => !string.IsNullOrWhiteSpace(value))
            .WithState(_ => ErrorCode.MISSING_VALUE)
            .WithMessage("{PropertyName} must be not null, empty or whitespace");

    public static IRuleBuilderOptions<T, string?> ValidOptionalString<T>(this IRuleBuilder<T, string?> ruleBuilder) =>
        ruleBuilder
            .Must(value => value == null || !string.IsNullOrWhiteSpace(value))
            .WithState(_ => ErrorCode.EMPTY_VALUE)
            .WithMessage("{PropertyName} must be null, or not empty or whitespace");

    public static IRuleBuilderOptions<T, string> ValidRequiredMaximumStringLength<T>(
        this IRuleBuilder<T, string> ruleBuilder, int maxLength) =>
        ruleBuilder
            .MaximumLength(maxLength)
            .WithState(_ => ErrorCode.MAX_LENGTH_EXCEEDED)
            .WithMessage("{PropertyName} must be not longer than {MaxLength} characters");

    public static IRuleBuilderOptions<T, string?> ValidOptionalMaximumStringLength<T>(
        this IRuleBuilder<T, string?> ruleBuilder, int maxLength) =>
        ruleBuilder
            .Must(value => value == null || value.Length <= maxLength)
            .WithState(_ => ErrorCode.MAX_LENGTH_EXCEEDED)
            .WithMessage($"{{PropertyName}} must be null or not longer than {maxLength} characters");
    
    public static IRuleBuilderOptions<T, string> ValidRequiredRegex<T>(this IRuleBuilder<T, string> ruleBuilder, string regex) =>
        ruleBuilder
            .Matches(regex)
            .WithState(_ => ErrorCode.PATTERN_MISMATCH)
            .WithMessage("{PropertyName} must match the required pattern");
    
    public static IRuleBuilderOptions<T, string> ValidRequiredRegex<T>(this IRuleBuilder<T, string> ruleBuilder, Func<T, string> regexSelector) =>
        ruleBuilder
            .Matches(regexSelector)
            .WithState(_ => ErrorCode.PATTERN_MISMATCH)
            .WithMessage("{PropertyName} must match the required pattern");
}
