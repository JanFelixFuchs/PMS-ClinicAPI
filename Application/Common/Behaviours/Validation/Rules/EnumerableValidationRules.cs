using FluentValidation;

namespace Application.Common.Behaviours.Validation.Rules;

public static class EnumerableValidationRules
{
    public static IRuleBuilderOptions<T, ICollection<TElement>> ValidRequiredCollection<T, TElement>(this IRuleBuilder<T, ICollection<TElement>> ruleBuilder) =>
        ruleBuilder
            .NotNull()
            .WithMessage("{PropertyName} must be not null");
    
    public static IRuleBuilderOptions<T, ICollection<TElement>> ValidRequiredDuplicateFreeCollection<T, TElement>(this IRuleBuilder<T, ICollection<TElement>> ruleBuilder) =>
        ruleBuilder
            .Must(value => value.Distinct().Count() == value.Count)
            .WithMessage("{PropertyName} must not contain duplicate elements");
    
    public static IRuleBuilderOptions<T, ICollection<TElement>> ValidRequiredMinimumCollectionLength<T, TElement>(this IRuleBuilder<T, ICollection<TElement>> ruleBuilder, int minLength = 1) =>
        ruleBuilder
            .Must(value => value.Count >= minLength)
            .WithMessage($"{{PropertyName}} must be not shorter than {minLength} elements");
    
    public static IRuleBuilderOptions<T, TElement[]> ValidRequiredArray<T, TElement>(this IRuleBuilder<T, TElement[]> ruleBuilder) =>
        ruleBuilder
            .NotNull()
            .WithMessage("{PropertyName} must be not null or empty");
    
    public static IRuleBuilderOptions<T, TElement[]> ValidRequiredMinimumArrayLength<T, TElement>(this IRuleBuilder<T, TElement[]> ruleBuilder, int minLength = 1) =>
        ruleBuilder
            .Must(value => value.Length >= minLength)
            .WithMessage($"{{PropertyName}} must be not shorter than {minLength} elements");
    
    public static IRuleBuilderOptions<T, TElement[]> ValidRequiredMaximumArrayLength<T, TElement>(this IRuleBuilder<T, TElement[]> ruleBuilder, int maxLength) =>
        ruleBuilder
            .Must(value => value.Length <= maxLength)
            .WithMessage($"{{PropertyName}} must be not longer than {maxLength} elements");
}