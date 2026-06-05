using FluentValidation;

namespace Application.Common.Behaviours.Validation.Rules;

public static class StringValidationRules
{
    public static IRuleBuilderOptions<T, string> ValidRequiredString<T>(this IRuleBuilder<T, string> ruleBuilder) =>
        ruleBuilder
            .Must(value => !string.IsNullOrWhiteSpace(value))
            .WithMessage("{PropertyName} must be not null, empty or whitespace");

    public static IRuleBuilderOptions<T, string?> ValidOptionalString<T>(this IRuleBuilder<T, string?> ruleBuilder) =>
        ruleBuilder
            .Must(value => value == null || !string.IsNullOrWhiteSpace(value))
            .WithMessage("{PropertyName} must be null, or not empty or whitespace");

    public static IRuleBuilderOptions<T, string> ValidRequiredMaximumStringLength<T>(
        this IRuleBuilder<T, string> ruleBuilder, int maxLength) =>
        ruleBuilder
            .MaximumLength(maxLength)
            .WithMessage("{PropertyName} must be not longer than {MaxLength} characters");

    public static IRuleBuilderOptions<T, string?> ValidOptionalMaximumStringLength<T>(
        this IRuleBuilder<T, string?> ruleBuilder, int maxLength) =>
        ruleBuilder
            .Must(value => value == null || value.Length <= maxLength)
            .WithMessage($"{{PropertyName}} must be null or not longer than {maxLength} characters");
    
    public static IRuleBuilderOptions<T, string> ValidRequiredRegex<T>(this IRuleBuilder<T, string> ruleBuilder, string regex) =>
        ruleBuilder
            .Matches(regex)
            .WithMessage("{PropertyName} must match the required pattern");
    
    public static IRuleBuilderOptions<T, string> ValidRequiredRegex<T>(this IRuleBuilder<T, string> ruleBuilder, Func<T, string> regexSelector) =>
        ruleBuilder
            .Matches(regexSelector)
            .WithMessage("{PropertyName} must match the required pattern");
}
