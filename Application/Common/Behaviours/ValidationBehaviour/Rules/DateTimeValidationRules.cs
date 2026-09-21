using FluentValidation;
using Utils.Exceptions.Errors.Codes;

namespace Application.Common.Behaviours.ValidationBehaviour.Rules;

public static class DateTimeValidationRules
{
    public static IRuleBuilderOptions<T, DateTime> ValidRequiredDateTime<T>(this IRuleBuilder<T, DateTime> ruleBuilder) =>
        ruleBuilder
            .NotEmpty()
            .WithState(_ => ErrorCode.MISSING_VALUE)
            .WithMessage("{PropertyName} must be not empty");
    
    public static IRuleBuilderOptions<T, DateTime> ValidRequiredPastDateTime<T>(this IRuleBuilder<T, DateTime> ruleBuilder, DateTime currentDateTime) =>
        ruleBuilder
            .LessThanOrEqualTo(_ => currentDateTime)
            .WithState(_ => ErrorCode.DATETIME_NOT_IN_PAST)
            .WithMessage("{PropertyName} must be in the past");
    
    public static IRuleBuilderOptions<T, DateTime> ValidRequiredBeforeDateTime<T>(this IRuleBuilder<T, DateTime> ruleBuilder, Func<T, DateTime> maxDateTimeSelector) =>
        ruleBuilder
            .Must((request, dateTime) => dateTime <= maxDateTimeSelector(request))
            .WithState(_ => ErrorCode.DATETIME_OUT_OF_RANGE)
            .WithMessage("{PropertyName} must be before than or equal to to the maximum allowed date time");
    
    public static IRuleBuilderOptions<T, DateTime> ValidRequiredFutureDateTime<T>(this IRuleBuilder<T, DateTime> ruleBuilder, DateTime currentDateTime) =>
        ruleBuilder
            .GreaterThanOrEqualTo(_ => currentDateTime)
            .WithState(_ => ErrorCode.DATETIME_NOT_IN_FUTURE)
            .WithMessage("{PropertyName} must be in the future");
    
    public static IRuleBuilderOptions<T, DateTime> ValidRequiredAfterDateTime<T>(this IRuleBuilder<T, DateTime> ruleBuilder, Func<T, DateTime> minDateTimeSelector) =>
        ruleBuilder
            .Must((request, dateTime) => dateTime >= minDateTimeSelector(request))
            .WithState(_ => ErrorCode.DATETIME_OUT_OF_RANGE)
            .WithMessage("{PropertyName} must be after than or equal to to the minimum allowed date time");
    
    public static IRuleBuilderOptions<T, DateTime?> ValidOptionalDateTime<T>(this IRuleBuilder<T, DateTime?> ruleBuilder) =>
        ruleBuilder
            .Must(value => value == null || value != default(DateTime))
            .WithState(_ => ErrorCode.EMPTY_VALUE)
            .WithMessage("{PropertyName} must be null or not empty");
    
    public static IRuleBuilderOptions<T, DateTime?> ValidOptionalPastDateTime<T>(this IRuleBuilder<T, DateTime?> ruleBuilder, DateTime currentDateTime) =>
        ruleBuilder
            .Must(value => value == null || value <= currentDateTime)
            .WithState(_ => ErrorCode.DATETIME_NOT_IN_PAST)
            .WithMessage("{PropertyName} must be null or in the past");
}