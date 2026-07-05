using FluentValidation;

namespace Application.Common.Behaviours.Validation.Rules;

public static class DateTimeValidationRules
{
    public static IRuleBuilderOptions<T, DateTime> ValidRequiredDateTime<T>(this IRuleBuilder<T, DateTime> ruleBuilder) =>
        ruleBuilder
            .NotEmpty()
            .WithMessage("{PropertyName} must be not empty");
    
    public static IRuleBuilderOptions<T, DateTime> ValidRequiredPastDateTime<T>(this IRuleBuilder<T, DateTime> ruleBuilder) =>
        ruleBuilder
            .LessThanOrEqualTo(_ => DateTime.UtcNow)
            .WithMessage("{PropertyName} must be in the past");
    
    public static IRuleBuilderOptions<T, DateTime> ValidRequiredBeforeDateTime<T>(this IRuleBuilder<T, DateTime> ruleBuilder, Func<T, DateTime> maxDateTimeSelector) =>
        ruleBuilder
            .Must((request, dateTime) => dateTime <= maxDateTimeSelector(request))
            .WithMessage("{PropertyName} must be before than or equal to to the maximum allowed date time");
    
    public static IRuleBuilderOptions<T, DateTime> ValidRequiredFutureDateTime<T>(this IRuleBuilder<T, DateTime> ruleBuilder) =>
        ruleBuilder
            .GreaterThanOrEqualTo(_ => DateTime.UtcNow)
            .WithMessage("{PropertyName} must be in the future");
    
    public static IRuleBuilderOptions<T, DateTime> ValidRequiredAfterDateTime<T>(this IRuleBuilder<T, DateTime> ruleBuilder, Func<T, DateTime> minDateTimeSelector) =>
        ruleBuilder
            .Must((request, dateTime) => dateTime >= minDateTimeSelector(request))
            .WithMessage("{PropertyName} must be after than or equal to to the minimum allowed date time");
    
    public static IRuleBuilderOptions<T, DateTime?> ValidOptionalDateTime<T>(this IRuleBuilder<T, DateTime?> ruleBuilder) =>
        ruleBuilder
            .Must(value => value == null || value != default(DateTime))
            .WithMessage("{PropertyName} must be null or not empty");
    
    public static IRuleBuilderOptions<T, DateTime?> ValidOptionalPastDateTime<T>(this IRuleBuilder<T, DateTime?> ruleBuilder) =>
        ruleBuilder
            .Must(value => value == null || value <= DateTime.UtcNow)
            .WithMessage("{PropertyName} must be null or in the past");
}