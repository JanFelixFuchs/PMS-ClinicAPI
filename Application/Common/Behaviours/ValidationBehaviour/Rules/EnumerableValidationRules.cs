using FluentValidation;
using Utils.Exceptions.Errors.Codes;

namespace Application.Common.Behaviours.ValidationBehaviour.Rules;

public static class EnumerableValidationRules
{
    public static IRuleBuilderOptions<T, ICollection<TElement>> ValidRequiredCollection<T, TElement>(this IRuleBuilder<T, ICollection<TElement>> ruleBuilder) =>
        ruleBuilder
            .NotNull()
            .WithState(_ => ErrorCode.MISSING_VALUE)
            .WithMessage("{PropertyName} must be not null");
    
    public static IRuleBuilderOptions<T, ICollection<TElement>> ValidRequiredDuplicateFreeCollection<T, TElement>(this IRuleBuilder<T, ICollection<TElement>> ruleBuilder) =>
        ruleBuilder
            .Must(value => value.Distinct().Count() == value.Count)
            .WithState(_ => ErrorCode.CONTAINS_DUPLICATE_ELEMENTS)
            .WithMessage("{PropertyName} must not contain duplicate elements");
    
    public static IRuleBuilderOptions<T, ICollection<TElement>> ValidRequiredMinimumCollectionLength<T, TElement>(this IRuleBuilder<T, ICollection<TElement>> ruleBuilder, int minLength = 1) =>
        ruleBuilder
            .Must(value => value.Count >= minLength)
            .WithState(_ => ErrorCode.MIN_LENGTH_NOT_REACHED)
            .WithMessage($"{{PropertyName}} must be not shorter than {minLength} elements");
    
    public static IRuleBuilderOptions<T, TElement[]> ValidRequiredArray<T, TElement>(this IRuleBuilder<T, TElement[]> ruleBuilder) =>
        ruleBuilder
            .NotNull()
            .WithState(_ => ErrorCode.MISSING_VALUE)
            .WithMessage("{PropertyName} must be not null or empty");
    
    public static IRuleBuilderOptions<T, TElement[]> ValidRequiredMinimumArrayLength<T, TElement>(this IRuleBuilder<T, TElement[]> ruleBuilder, int minLength = 1) =>
        ruleBuilder
            .Must(value => value.Length >= minLength)
            .WithState(_ => ErrorCode.MIN_LENGTH_NOT_REACHED)
            .WithMessage($"{{PropertyName}} must be not shorter than {minLength} elements");
    
    public static IRuleBuilderOptions<T, TElement[]> ValidRequiredMaximumArrayLength<T, TElement>(this IRuleBuilder<T, TElement[]> ruleBuilder, int maxLength) =>
        ruleBuilder
            .Must(value => value.Length <= maxLength)
            .WithState(_ => ErrorCode.MAX_LENGTH_EXCEEDED)
            .WithMessage($"{{PropertyName}} must be not longer than {maxLength} elements");
}