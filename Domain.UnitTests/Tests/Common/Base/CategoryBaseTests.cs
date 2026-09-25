using Domain.Common.Utils.Constants;
using Domain.Tests.Utils.Builders.CategoryBuilders;
using Domain.Tests.Utils.Models.Category;
using FluentAssertions;
using TestUtils.Constants;
using Utils.Exceptions.CustomExceptions;
using Utils.Exceptions.Errors.Codes;
using InvalidOperationException = Domain.Common.Exceptions.InvalidOperationException;

namespace Domain.Tests.Tests.Common.Base;

public class CategoryBaseTests
{
    /* - - - Preparation - - - */
    public static TheoryData<string> InvalidColorsNotMatchingRegex =>
    [
        "000",
        "000000",
        "#00",
        "#0000",
        "#0000000",
        "#GGG",
        "#GGGGGG"
    ];
    
    
    /* - - - Constructor - - - */
    [Fact]
    public void Constructor_WithNullClinic_ThrowsValidationException()
    {
        // Act
        var act = () => TestCategoryBuilder
            .Create()
            .WithClinic(null)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(TestCategory.Clinic),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }

    [Theory]
    [MemberData(nameof(TestConstants.InvalidNullEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void Constructor_WithNullEmptyOrWhitespaceName_ThrowsValidationException(string? name)
    {
        // Act
        var act = () => TestCategoryBuilder
            .Create()
            .WithName(name)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(TestCategory.Name),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }

    [Fact]
    public void Constructor_WithNameExceedingMaximumLength_ThrowsValidationException()
    {
        // Arrange
        var name = new string('*', Lengths.CategoryName + 1);
        
        // Act
        var act = () => TestCategoryBuilder
            .Create()
            .WithName(name)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(TestCategory.Name),
            ErrorCode = ErrorCode.MAX_LENGTH_EXCEEDED
        });
    }
    
    [Theory]
    [MemberData(nameof(TestConstants.InvalidNullEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void Constructor_WithNullEmptyOrWhitespaceAbbreviation_ThrowsValidationException(string? abbreviation)
    {
        // Act
        var act = () => TestCategoryBuilder
            .Create()
            .WithAbbreviation(abbreviation)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(TestCategory.Abbreviation),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }

    [Fact]
    public void Constructor_WithAbbreviationExceedingMaximumLength_ThrowsValidationException()
    {
        // Arrange
        var abbreviation = new string('*', Lengths.Abbreviation + 1);
        
        // Act
        var act = () => TestCategoryBuilder
            .Create()
            .WithAbbreviation(abbreviation)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(TestCategory.Abbreviation),
            ErrorCode = ErrorCode.MAX_LENGTH_EXCEEDED
        });
    }
    
    [Theory]
    [MemberData(nameof(TestConstants.InvalidNullEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void Constructor_WithNullEmptyOrWhitespaceColor_ThrowsValidationException(string? color)
    {
        // Act
        var act = () => TestCategoryBuilder
            .Create()
            .WithColor(color)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(TestCategory.Color),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }

    [Theory]
    [MemberData(nameof(InvalidColorsNotMatchingRegex))]
    public void Constructor_WithColorNotMatchingRegex_ThrowsValidationException(string color)
    {
        // Act
        var act = () => TestCategoryBuilder
            .Create()
            .WithColor(color)
            .Build();
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(TestCategory.Color),
            ErrorCode = ErrorCode.PATTERN_MISMATCH
        });
    }
    
    [Fact]
    public void Constructor_WithValidArguments_SetsAllProperties()
    {
        // Arrange
        var categoryBuilder = TestCategoryBuilder.Create();
        
        // Act
        var sut = categoryBuilder.Build();
        
        // Assert
        sut.Id.Should().NotBeEmpty();
        sut.Clinic.Should().Be(categoryBuilder.Clinic);
        sut.ClinicId.Should().Be(categoryBuilder.Clinic!.Id);
        sut.Name.Should().Be(categoryBuilder.Name);
        sut.Abbreviation.Should().Be(categoryBuilder.Abbreviation);
        sut.Color.Should().Be(categoryBuilder.Color);
        sut.IsDeleted.Should().BeFalse();
        sut.CategoryItems.Should().BeEmpty();
    }
    
    
    /* - - - Method: Update - - - */
    [Fact]
    public void Update_WithDeletedCategory_ThrowsInvalidOperationException()
    {
        // Arrange
        var categoryBuilder = TestCategoryBuilder.Create();
        var sut = categoryBuilder.Build();
        sut.Delete([]);
        
        // Act
        var act = categoryBuilder
            .AsUpdate()
            .Apply(sut);
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
    
    [Theory]
    [MemberData(nameof(TestConstants.InvalidNullEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void Update_WithNullEmptyOrWhitespaceName_ThrowsValidationException(string? name)
    {
        // Arrange
        var categoryBuilder = TestCategoryBuilder.Create();
        var sut = categoryBuilder.Build();
        
        // Act
        var act = categoryBuilder
            .AsUpdate()
            .WithName(name)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(TestCategory.Name),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }

    [Fact]
    public void Update_WithNameExceedingMaximumLength_ThrowsValidationException()
    {
        // Arrange
        var name = new string('*', Lengths.CategoryName + 1);
        var categoryBuilder = TestCategoryBuilder.Create();
        var sut = categoryBuilder.Build();
        
        // Act
        var act = categoryBuilder
            .AsUpdate()
            .WithName(name)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(TestCategory.Name),
            ErrorCode = ErrorCode.MAX_LENGTH_EXCEEDED
        });
    }
    
    [Theory]
    [MemberData(nameof(TestConstants.InvalidNullEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void Update_WithNullEmptyOrWhitespaceAbbreviation_ThrowsValidationException(string? abbreviation)
    {
        // Arrange
        var categoryBuilder = TestCategoryBuilder.Create();
        var sut = categoryBuilder.Build();
        
        // Act
        var act = categoryBuilder
            .AsUpdate()
            .WithAbbreviation(abbreviation)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(TestCategory.Abbreviation),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }

    [Fact]
    public void Update_WithAbbreviationExceedingMaximumLength_ThrowsValidationException()
    {
        // Arrange
        var abbreviation = new string('*', Lengths.Abbreviation + 1);
        var categoryBuilder = TestCategoryBuilder.Create();
        var sut = categoryBuilder.Build();
        
        // Act
        var act = categoryBuilder
            .AsUpdate()
            .WithAbbreviation(abbreviation)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(TestCategory.Abbreviation),
            ErrorCode = ErrorCode.MAX_LENGTH_EXCEEDED
        });
    }
    
    [Theory]
    [MemberData(nameof(TestConstants.InvalidNullEmptyOrWhitespaceString), MemberType = typeof(TestConstants))]
    public void Update_WithNullEmptyOrWhitespaceColor_ThrowsValidationException(string? color)
    {
        // Arrange
        var categoryBuilder = TestCategoryBuilder.Create();
        var sut = categoryBuilder.Build();
        
        // Act
        var act = categoryBuilder
            .AsUpdate()
            .WithColor(color)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(TestCategory.Color),
            ErrorCode = ErrorCode.MISSING_VALUE
        });
    }

    [Theory]
    [MemberData(nameof(InvalidColorsNotMatchingRegex))]
    public void Update_WithColorNotMatchingRegex_ThrowsValidationException(string color)
    {
        // Arrange
        var categoryBuilder = TestCategoryBuilder.Create();
        var sut = categoryBuilder.Build();
        
        // Act
        var act = categoryBuilder
            .AsUpdate()
            .WithColor(color)
            .Apply(sut);
        
        // Assert
        var exception = act.Should().Throw<ValidationException>().Which;
        exception.FieldErrors.Should().ContainEquivalentOf(new
        {
            Field = nameof(TestCategory.Color),
            ErrorCode = ErrorCode.PATTERN_MISMATCH
        });
    }
    
    [Fact]
    public void Update_WithValidArguments_UpdatesAllProperties()
    {
        // Assert
        var categoryBuilder = TestCategoryBuilder.Create();
        var sut = categoryBuilder.Build();
        
        // Act
        categoryBuilder.AsUpdate().Apply(sut)();
        
        // Assert
        sut.Name.Should().Be(categoryBuilder.Name);
        sut.Abbreviation.Should().Be(categoryBuilder.Abbreviation);
        sut.Color.Should().Be(categoryBuilder.Color);
    }
    
    
    /* - - - Method: Delete - - - */
    [Fact]
    public void Delete_WithDeletedCategory_ThrowsInvalidOperationException()
    {
        // Arrange
        var sut = TestCategoryBuilder.Create().Build();
        sut.Delete([]);
        
        // Act
        var act = () => sut.Delete([]);
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Delete_WithExistingItems_ThrowsInvalidOperationException()
    {
        // Arrange
        var sut = TestCategoryBuilder.Create().Build();
        
        // Act
        var act = () => sut.Delete([new TestCategoryItem()]);
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Delete_WithDeletableCategory_DeletesCategory()
    {
        // Arrange
        var sut = TestCategoryBuilder.Create().Build();
        
        // Act
        sut.Delete([]);
        
        // Assert
        sut.IsDeleted.Should().BeTrue();
    } 
    
    
    /* - - - Method: ToString - - - */
    [Fact]
    public void ToString_ReturnsStringContainingAllProperties()
    {
        // Arrange
        var sut = TestCategoryBuilder.Create().Build();
    
        // Act
        var result = sut.ToString();
        
        // Assert
        result.Should()
            .Contain(sut.Id.ToString()).And
            .Contain(sut.ClinicId.ToString()).And
            .Contain(sut.Name).And
            .Contain(sut.Abbreviation).And
            .Contain(sut.Color).And
            .Contain(sut.IsDeleted.ToString());
    }
    
    
    /* - - - Method: Equals and GetHashCode - - - */
    [Fact]
    public void Equals_WithNull_ReturnsFalse()
    {
        // Arrange
        var sut = TestCategoryBuilder.Create().Build();
        
        // Act
        var result = sut.Equals(null);
        
        // Assert
        result.Should().BeFalse();
    }
    
    [Fact]
    public void Equals_WithDifferentType_ReturnsFalse()
    {
        // Arrange
        var sut = TestCategoryBuilder.Create().Build();
        
        // Act
        var result = sut.Equals(new object());
        
        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Equals_WithDifferentId_ReturnsFalse()
    {
        // Arrange
        var categoryOne = TestCategoryBuilder.Create().Build();
        var categoryTwo = TestCategoryBuilder.Create().Build();
        
        // Act
        var result = categoryOne.Equals(categoryTwo);
        
        // Assert
        result.Should().BeFalse();
    }
    
    [Fact]
    public void Equals_WithSameId_ReturnsTrue()
    {
        // Arrange
        var sut = TestCategoryBuilder.Create().Build();
        var categoryOne = sut;
        var categoryTwo = sut;
        
        // Act
        var equalsResult = sut.Equals(categoryOne);
        var getHashCodeCategoryOneResult = categoryOne.GetHashCode();
        var getHashCodeCategoryTwoResult = categoryTwo.GetHashCode();
        
        // Assert
        equalsResult.Should().BeTrue();
        getHashCodeCategoryOneResult.Should().Be(getHashCodeCategoryTwoResult);
    }
}