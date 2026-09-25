using Domain.Common.Utils.InvariantValidation;
using Domain.Tests.Utils.Models.Interfaces;
using FluentAssertions;
using TestUtils.Models;
using Utils.Exceptions.Errors.Codes;

namespace Domain.Tests.Tests.Common.Utils.InvariantValidation;

public class InvariantValidationConditionsTests
{
    /* - - - Preparation - - - */
    private const string PropertyName = "test-property";
    
    
    /* - - - Method: IsNotDeleted - - - */
    [Fact]
    public void IsNotDeleted_WithDeletedEntity_ReturnsInvalidInvariantValidationResult()
    {
        // Act
        var result = InvariantValidationConditions.IsNotDeleted(new TestDeletable(true), PropertyName);
        
        // Assert
        result.IsValid.Should().BeFalse();
        result.Field.Should().Be(PropertyName);
        result.ErrorCode.Should().Be(ErrorCode.DELETED_ENTITY);
    }
    
    [Fact]
    public void IsNotDeleted_WithNonDeletedEntity_ReturnsValidInvariantValidationResult()
    {
        // Act
        var result = InvariantValidationConditions.IsNotDeleted(new TestDeletable(false), PropertyName);
        
        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    
    /* - - - Method: IsNullOrNotDeleted - - - */
    [Fact]
    public void IsNullOrNotDeleted_WithDeletedEntity_ReturnsInvalidInvariantValidationResult()
    {
        // Act
        var result = InvariantValidationConditions.IsNullOrNotDeleted(new TestDeletable(true), PropertyName);
        
        // Assert
        result.IsValid.Should().BeFalse();
        result.Field.Should().Be(PropertyName);
        result.ErrorCode.Should().Be(ErrorCode.DELETED_ENTITY);
    }
    
    [Fact]
    public void IsNullOrNotDeleted_WithNull_ReturnsValidInvariantValidationResult()
    {
        // Act
        var result = InvariantValidationConditions.IsNullOrNotDeleted<TestDeletable>(null, PropertyName);
        
        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    [Fact]
    public void IsNullOrNotDeleted_WithNonDeletedEntity_ReturnsValidInvariantValidationResult()
    {
        // Act
        var result = InvariantValidationConditions.IsNullOrNotDeleted(new TestDeletable(false), PropertyName);
        
        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    
    /* - - - Method: IsNotArchived - - - */
    [Fact]
    public void IsNotArchived_WithArchivedEntity_ReturnsInvalidInvariantValidationResult()
    {
        // Act
        var result = InvariantValidationConditions.IsNotArchived(new TestArchivable(true), PropertyName);
        
        // Assert
        result.IsValid.Should().BeFalse();
        result.Field.Should().Be(PropertyName);
        result.ErrorCode.Should().Be(ErrorCode.ARCHIVED_ENTITY);
    }
    
    [Fact]
    public void IsNotArchived_WithNonArchivedEntity_ReturnsValidInvariantValidationResult()
    {
        // Act
        var result = InvariantValidationConditions.IsNotArchived(new TestArchivable(false), PropertyName);
        
        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    
    /* - - - Method: IsNullOrNotArchived - - - */
    [Fact]
    public void IsNullOrNotArchived_WithArchivedEntity_ReturnsInvalidInvariantValidationResult()
    {
        // Act
        var result = InvariantValidationConditions.IsNullOrNotArchived(new TestArchivable(true), PropertyName);
        
        // Assert
        result.IsValid.Should().BeFalse();
        result.Field.Should().Be(PropertyName);
        result.ErrorCode.Should().Be(ErrorCode.ARCHIVED_ENTITY);
    }
    
    [Fact]
    public void IsNullOrNotArchived_WithNull_ReturnsValidInvariantValidationResult()
    {
        // Act
        var result = InvariantValidationConditions.IsNullOrNotArchived<TestArchivable>(null, PropertyName);
        
        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    [Fact]
    public void IsNullOrNotArchived_WithNonDeletedEntity_ReturnsValidInvariantValidationResult()
    {
        // Act
        var result = InvariantValidationConditions.IsNullOrNotArchived(new TestArchivable(false), PropertyName);
        
        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    
    /* - - - Method: IsExactEnumValue - - - */
    [Fact]
    public void IsExactEnumValue_WithNonMatchingValue_ReturnsInvalidInvariantValidationResult()
    {
        // Act
        var result = InvariantValidationConditions.IsExactEnumValue(TestEnum.ValueOne, TestEnum.ValueTwo, PropertyName);
        
        // Assert
        result.IsValid.Should().BeFalse();
        result.Field.Should().Be(PropertyName);
        result.ErrorCode.Should().Be(ErrorCode.UNEXPECTED_STATUS);
    }
    
    [Fact]
    public void IsExactEnumValue_WithMatchingValue_ReturnsValidInvariantValidationResult()
    {
        // Act
        var result = InvariantValidationConditions.IsExactEnumValue(TestEnum.ValueOne, TestEnum.ValueOne, PropertyName);
        
        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    
    /* - - - Method: IsBelongingToSameClinic - - - */
    [Fact]
    public void IsBelongingToSameClinic_WithNonMatchingValue_ReturnsInvalidInvariantValidationResult()
    {
        // Act
        var result = InvariantValidationConditions.IsBelongingToSameClinic(Guid.NewGuid(), Guid.Empty, PropertyName);
        
        // Assert
        result.IsValid.Should().BeFalse();
        result.Field.Should().Be(PropertyName);
        result.ErrorCode.Should().Be(ErrorCode.CLINIC_MISMATCH);
    }
    
    [Fact]
    public void IsBelongingToSameClinic_WithMatchingValue_ReturnsValidInvariantValidationResult()
    {
        // Arrange
        var guid = Guid.NewGuid();
        
        // Act
        var result = InvariantValidationConditions.IsBelongingToSameClinic(guid, guid, PropertyName);
        
        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    
    /* - - - Method: IsNullOrBelongingToSameClinic - - - */
    [Fact]
    public void IsNullOrBelongingToSameClinic_WithNonMatchingValue_ReturnsInvalidInvariantValidationResult()
    {
        // Act
        var result = InvariantValidationConditions.IsNullOrBelongingToSameClinic(Guid.NewGuid(), Guid.Empty, PropertyName);
        
        // Assert
        result.IsValid.Should().BeFalse();
        result.Field.Should().Be(PropertyName);
        result.ErrorCode.Should().Be(ErrorCode.CLINIC_MISMATCH);
    }
    
    [Fact]
    public void IsNullOrBelongingToSameClinic_WithNull_ReturnsValidInvariantValidationResult()
    {
        // Act
        var result = InvariantValidationConditions.IsNullOrBelongingToSameClinic(null, Guid.Empty, PropertyName);
        
        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    [Fact]
    public void IsNullOrBelongingToSameClinic_WithMatchingValue_ReturnsValidInvariantValidationResult()
    {
        // Arrange
        var guid = Guid.NewGuid();
        
        // Act
        var result = InvariantValidationConditions.IsNullOrBelongingToSameClinic(guid, guid, PropertyName);
        
        // Assert
        result.IsValid.Should().BeTrue();
    }

    
    /* - - - Method: IsNotContainingDeletedElements - - - */
    [Fact]
    public void IsNotContainingDeletedElements_WithDeletedElement_ReturnsInvalidInvariantValidationResult()
    {
        // Arrange
        var collectionWithDeletedElement = new List<TestDeletable> { new(false), new(true) };
        
        // Act
        var result = InvariantValidationConditions.IsNotContainingDeletedElements(collectionWithDeletedElement, PropertyName);
        
        // Assert
        result.IsValid.Should().BeFalse();
        result.Field.Should().Be(PropertyName);
        result.ErrorCode.Should().Be(ErrorCode.DELETED_ENTITY);
    }
    
    [Fact]
    public void IsNotContainingDeletedElements_WithEmptyCollection_ReturnsValidInvariantValidationResult()
    {
        // Act
        var result = InvariantValidationConditions.IsNotContainingDeletedElements(new List<TestDeletable>() , PropertyName);
        
        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    [Fact]
    public void IsNotContainingDeletedElements_WithNoDeletedElements_ReturnsValidInvariantValidationResult()
    {
        // Arrange
        var collectionWithNoDeletedElements = new List<TestDeletable> { new(false), new(false) };
        
        // Act
        var result = InvariantValidationConditions.IsNotContainingDeletedElements(collectionWithNoDeletedElements , PropertyName);
        
        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    
    /* - - - Method: IsNotContainingArchivedElements - - - */
    [Fact]
    public void IsNotContainingArchivedElements_WithArchivedElement_ReturnsInvalidInvariantValidationResult()
    {
        // Arrange
        var collectionWithArchivedElement = new List<TestArchivable> { new(false), new(true) };
        
        // Act
        var result = InvariantValidationConditions.IsNotContainingArchivedElements(collectionWithArchivedElement, PropertyName);
        
        // Assert
        result.IsValid.Should().BeFalse();
        result.Field.Should().Be(PropertyName);
        result.ErrorCode.Should().Be(ErrorCode.ARCHIVED_ENTITY);
    }
    
    [Fact]
    public void IsNotContainingArchivedElements_WithEmptyCollection_ReturnsValidInvariantValidationResult()
    {
        // Act
        var result = InvariantValidationConditions.IsNotContainingArchivedElements(new List<TestArchivable>() , PropertyName);
        
        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    [Fact]
    public void IsNotContainingArchivedElements_WithNoArchivedElements_ReturnsValidInvariantValidationResult()
    {
        // Arrange
        var collectionWithNoArchivedElements = new List<TestArchivable> { new(false), new(false) };
        
        // Act
        var result = InvariantValidationConditions.IsNotContainingArchivedElements(collectionWithNoArchivedElements , PropertyName);
        
        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    
    /* - - - Method: IsContainingOnlyElementsWithExactEnumValue - - - */
    [Fact]
    public void IsContainingOnlyElementsWithExactEnumValue_WithNonMatchingElements_ReturnsInvalidInvariantValidationResult()
    {
        // Arrange
        var collectionWithNonMatchingElements = new List<TestEnum> { TestEnum.ValueOne, TestEnum.ValueTwo };
        
        // Act
        var result = InvariantValidationConditions.IsContainingOnlyElementsWithExactEnumValue(
            collectionWithNonMatchingElements,
            enumValue => enumValue,
            TestEnum.ValueOne,
            PropertyName);
        
        // Assert
        result.IsValid.Should().BeFalse();
        result.Field.Should().Be(PropertyName);
        result.ErrorCode.Should().Be(ErrorCode.UNEXPECTED_STATUS);
    }
    
    [Fact]
    public void IsContainingOnlyElementsWithExactEnumValue_WithEmptyCollection_ReturnsValidInvariantValidationResult()
    {
        // Act
        var result = InvariantValidationConditions.IsContainingOnlyElementsWithExactEnumValue(
            new List<TestEnum>(),
            enumValue => enumValue,
            TestEnum.ValueOne,
            PropertyName);
        
        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    [Fact]
    public void IsContainingOnlyElementsWithExactEnumValue_WithAllMatchingElements_ReturnsValidInvariantValidationResult()
    {
        // Arrange
        var collectionWithAllMatchingElements = new List<TestEnum> { TestEnum.ValueOne, TestEnum.ValueOne };
        
        // Act
        var result = InvariantValidationConditions.IsContainingOnlyElementsWithExactEnumValue(
            collectionWithAllMatchingElements,
            enumValue => enumValue,
            TestEnum.ValueOne,
            PropertyName);
        
        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    
    /* - - - Method: IsContainingOnlyElementsWithExactGuidValue - - - */
    [Fact]
    public void IsContainingOnlyElementsWithExactGuidValue_WithNonMatchingElements_ReturnsInvalidInvariantValidationResult()
    {
        // Arrange
        var expectedGuid = Guid.NewGuid();
        var collectionWithNonMatchingElements = new List<Guid> { expectedGuid, Guid.Empty };
        
        // Act
        var result = InvariantValidationConditions.IsContainingOnlyElementsBelongingToSameClinic(
            collectionWithNonMatchingElements,
            guidValue => guidValue,
            expectedGuid,
            PropertyName);
        
        // Assert
        result.IsValid.Should().BeFalse();
        result.Field.Should().Be(PropertyName);
        result.ErrorCode.Should().Be(ErrorCode.CLINIC_MISMATCH);
    }
    
    [Fact]
    public void IsContainingOnlyElementsWithExactGuidValue_WithEmptyCollection_ReturnsValidInvariantValidationResult()
    {
        // Act
        var result = InvariantValidationConditions.IsContainingOnlyElementsBelongingToSameClinic(
            new List<Guid>(),
            guidValue => guidValue,
            Guid.Empty,
            PropertyName);
        
        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    [Fact]
    public void IsContainingOnlyElementsWithExactGuidValue_WithAllMatchingElements_ReturnsValidInvariantValidationResult()
    {
        // Arrange
        var expectedGuid = Guid.NewGuid();
        var collectionWithAllMatchingElements = new List<Guid> { expectedGuid, expectedGuid };
        
        // Act
        var result = InvariantValidationConditions.IsContainingOnlyElementsBelongingToSameClinic(
            collectionWithAllMatchingElements,
            guidValue => guidValue,
            expectedGuid,
            PropertyName);
        
        // Assert
        result.IsValid.Should().BeTrue();
    }
}