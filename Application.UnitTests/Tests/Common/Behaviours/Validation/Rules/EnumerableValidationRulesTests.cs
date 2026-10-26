using Application.Common.Behaviours.ValidationBehaviour.Rules;
using FluentValidation;
using FluentValidation.TestHelper;
using Utils.Exceptions.Errors.Codes;

namespace Application.UnitTests.Tests.Common.Behaviours.Validation.Rules;

public class EnumerableValidationRulesTests
{
    /* - - - Preparation - - - */
    private class EnumerableValidationRulesTestModel
    {
        public ICollection<Guid> Collection { get; init; } = new List<Guid>();
        public int[] Array { get; init; } = [];
    }
    
    private class RequiredCollectionValidator : AbstractValidator<EnumerableValidationRulesTestModel>
    {
        public RequiredCollectionValidator() => 
            RuleFor(model => model.Collection).ValidRequiredCollection();
    }

    private class DuplicateFreeCollectionValidator : AbstractValidator<EnumerableValidationRulesTestModel>
    {
        public DuplicateFreeCollectionValidator() => 
            RuleFor(model => model.Collection).ValidRequiredDuplicateFreeCollection();
    }
    
    private class MinimumCollectionLengthValidator : AbstractValidator<EnumerableValidationRulesTestModel>
    {
        public MinimumCollectionLengthValidator(int minLength) =>
            RuleFor(model => model.Collection).ValidRequiredMinimumCollectionLength(minLength);
        
        public MinimumCollectionLengthValidator() =>
            RuleFor(model => model.Collection).ValidRequiredMinimumCollectionLength();
    }
    
    private class RequiredArrayValidator : AbstractValidator<EnumerableValidationRulesTestModel>
    {
        public RequiredArrayValidator() => 
            RuleFor(model => model.Array).ValidRequiredArray();
    }
    
    private class MinimumArrayLengthValidator : AbstractValidator<EnumerableValidationRulesTestModel>
    {
        public MinimumArrayLengthValidator(int minLength) =>
            RuleFor(model => model.Array).ValidRequiredMinimumArrayLength(minLength);
        
        public MinimumArrayLengthValidator() =>
            RuleFor(model => model.Array).ValidRequiredMinimumArrayLength();
    }
    
    private class MaximumArrayLengthValidator : AbstractValidator<EnumerableValidationRulesTestModel>
    {
        public MaximumArrayLengthValidator(int maxLength) =>
            RuleFor(model => model.Array).ValidRequiredMaximumArrayLength(maxLength);
    }
    
    
    /* - - - Method: ValidRequiredCollection - - - */
    [Fact]
    public void ValidRequiredCollection_WithNull_HasValidationErrorWithMissingValueState()
    {
        // Arrange
        var sut = new RequiredCollectionValidator();
        var model = new EnumerableValidationRulesTestModel { Collection = null! };

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result
            .ShouldHaveValidationErrorFor(testModel => testModel.Collection)
            .WithCustomState(ErrorCode.MISSING_VALUE);
    }

    [Fact]
    public void ValidRequiredCollection_WithEmptyCollection_HasNoValidationError()
    {
        // Arrange
        var sut = new RequiredCollectionValidator();
        var model = new EnumerableValidationRulesTestModel { Collection = [] };

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(testModel => testModel.Collection);
    }

    [Fact]
    public void ValidRequiredCollection_WithPopulatedCollection_HasNoValidationError()
    {
        // Arrange
        var sut = new RequiredCollectionValidator();
        var model = new EnumerableValidationRulesTestModel { Collection = [Guid.NewGuid()] };

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(testModel => testModel.Collection);
    }
    
    
    /* - - - Method: ValidRequiredDuplicateFreeCollection - - - */
    [Fact]
    public void ValidRequiredDuplicateFreeCollection_WithDuplicateElements_HasValidationErrorWithDuplicateElementsState()
    {
        // Arrange
        var sut = new DuplicateFreeCollectionValidator();
        var duplicateId = Guid.NewGuid();
        var model = new EnumerableValidationRulesTestModel { Collection = [duplicateId, duplicateId] };

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result
            .ShouldHaveValidationErrorFor(testModel => testModel.Collection)
            .WithCustomState(ErrorCode.CONTAINS_DUPLICATE_ELEMENTS);
    }
    
    [Fact]
    public void ValidRequiredDuplicateFreeCollection_WithEmptyCollection_HasNoValidationError()
    {
        // Arrange
        var sut = new DuplicateFreeCollectionValidator();
        var model = new EnumerableValidationRulesTestModel { Collection = [] };

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(testModel => testModel.Collection);
    }
    
    [Fact]
    public void ValidRequiredDuplicateFreeCollection_WithUniqueElements_HasNoValidationError()
    {
        // Arrange
        var sut = new DuplicateFreeCollectionValidator();
        var model = new EnumerableValidationRulesTestModel { Collection = [Guid.NewGuid(), Guid.NewGuid()] };

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(testModel => testModel.Collection);
    }
    
    
    /* - - - Method: ValidRequiredMinimumCollectionLength - - - */
    [Fact]
    public void ValidRequiredMinimumCollectionLength_WithCollectionBelowMinLength_HasValidationErrorWithMinLengthNotReachedState()
    {
        // Arrange
        var sut = new MinimumCollectionLengthValidator(minLength: 2);
        var model = new EnumerableValidationRulesTestModel { Collection = [Guid.NewGuid()] };

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result
            .ShouldHaveValidationErrorFor(testModel => testModel.Collection)
            .WithCustomState(ErrorCode.MIN_LENGTH_NOT_REACHED);
    }

    [Fact]
    public void ValidRequiredMinimumCollectionLength_WithDefaultMinLength_UsesOneAsMinimum()
    {
        // Arrange
        var sut = new MinimumCollectionLengthValidator();
        var model = new EnumerableValidationRulesTestModel { Collection = [] };

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result
            .ShouldHaveValidationErrorFor(testModel => testModel.Collection)
            .WithCustomState(ErrorCode.MIN_LENGTH_NOT_REACHED);
    }
    
    [Fact]
    public void ValidRequiredMinimumCollectionLength_WithCollectionAtMinLength_HasNoValidationError()
    {
        // Arrange
        var sut = new MinimumCollectionLengthValidator(minLength: 2);
        var model = new EnumerableValidationRulesTestModel { Collection = [Guid.NewGuid(), Guid.NewGuid()] };

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(testModel => testModel.Collection);
    }

    [Fact]
    public void ValidRequiredMinimumCollectionLength_WithCollectionAboveMinLength_HasNoValidationError()
    {
        // Arrange
        var sut = new MinimumCollectionLengthValidator(minLength: 1);
        var model = new EnumerableValidationRulesTestModel { Collection = [Guid.NewGuid(), Guid.NewGuid()] };

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(testModel => testModel.Collection);
    }
    
    
    /* - - - Method: ValidRequiredArray - - - */
    [Fact]
    public void ValidRequiredArray_WithNull_HasValidationErrorWithMissingValueState()
    {
        // Arrange
        var sut = new RequiredArrayValidator();
        var model = new EnumerableValidationRulesTestModel { Array = null! };

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result
            .ShouldHaveValidationErrorFor(testModel => testModel.Array)
            .WithCustomState(ErrorCode.MISSING_VALUE);
    }
    
    [Fact]
    public void ValidRequiredArray_WithEmptyArray_HasNoValidationError()
    {
        // Arrange
        var sut = new RequiredArrayValidator();
        var model = new EnumerableValidationRulesTestModel { Array = [] };

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(testModel => testModel.Array);
    }

    [Fact]
    public void ValidRequiredArray_WithPopulatedArray_HasNoValidationError()
    {
        // Arrange
        var sut = new RequiredArrayValidator();
        var model = new EnumerableValidationRulesTestModel { Array = [1, 2] };

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(testModel => testModel.Array);
    }
    
    
    /* - - - Method: ValidRequiredMinimumArrayLength - - - */
    [Fact]
    public void ValidRequiredMinimumArrayLength_WithArrayBelowMinLength_HasValidationErrorWithMinLengthNotReachedState()
    {
        // Arrange
        var sut = new MinimumArrayLengthValidator(minLength: 2);
        var model = new EnumerableValidationRulesTestModel { Array = [1] };

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result
            .ShouldHaveValidationErrorFor(testModel => testModel.Array)
            .WithCustomState(ErrorCode.MIN_LENGTH_NOT_REACHED);
    }

    [Fact]
    public void ValidRequiredMinimumArrayLength_WithDefaultMinLength_UsesOneAsMinimum()
    {
        // Arrange
        var sut = new MinimumArrayLengthValidator();
        var model = new EnumerableValidationRulesTestModel { Array = [] };

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result
            .ShouldHaveValidationErrorFor(testModel => testModel.Array)
            .WithCustomState(ErrorCode.MIN_LENGTH_NOT_REACHED);
    }
    
    [Fact]
    public void ValidRequiredMinimumArrayLength_WithArrayAtMinLength_HasNoValidationError()
    {
        // Arrange
        var sut = new MinimumArrayLengthValidator(minLength: 2);
        var model = new EnumerableValidationRulesTestModel { Array = [1, 2] };

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(m => m.Array);
    }
    
    [Fact]
    public void ValidRequiredMinimumArrayLength_WithArrayAboveMinLength_HasNoValidationError()
    {
        // Arrange
        var sut = new MinimumArrayLengthValidator(minLength: 1);
        var model = new EnumerableValidationRulesTestModel { Array = [1, 2] };

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(testModel => testModel.Collection);
    }
    
    
    /* - - - Method: ValidRequiredMaximumArrayLength - - - */
    [Fact]
    public void ValidRequiredMaximumArrayLength_WithArrayAboveMaxLength_HasValidationErrorWithMaxLengthExceededState()
    {
        // Arrange
        var sut = new MaximumArrayLengthValidator(maxLength: 1);
        var model = new EnumerableValidationRulesTestModel { Array = [1, 2] };

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result
            .ShouldHaveValidationErrorFor(testModel => testModel.Array)
            .WithCustomState(ErrorCode.MAX_LENGTH_EXCEEDED);
    }
    
    [Fact]
    public void ValidRequiredMaximumArrayLength_WithArrayAtMaxLength_HasNoValidationError()
    {
        // Arrange
        var sut = new MaximumArrayLengthValidator(maxLength: 2);
        var model = new EnumerableValidationRulesTestModel { Array = [1, 2] };

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(m => m.Array);
    }

    [Fact]
    public void ValidRequiredMaximumArrayLength_WithArrayBelowMaxLength_HasNoValidationError()
    {
        // Arrange
        var sut = new MaximumArrayLengthValidator(maxLength: 3);
        var model = new EnumerableValidationRulesTestModel { Array = [1, 2] };

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(m => m.Array);
    }
}