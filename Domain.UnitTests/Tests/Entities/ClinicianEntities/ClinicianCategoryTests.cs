using FluentAssertions;
using TestUtils.Builders.ClinicianBuilders;
using InvalidOperationException = Domain.Common.Exceptions.InvalidOperationException;

namespace Domain.Tests.Tests.Entities.ClinicianEntities;

public class ClinicianCategoryTests
{
    /* - - - Constructor - - - */
    [Fact]
    public void Constructor_WithValidArguments_SetsAllProperties()
    {
        // Arrange
        var clinicianCategoryBuilder = TestClinicianCategoryBuilder.Create();
        
        // Act
        var sut = clinicianCategoryBuilder.Build();
        
        // Assert
        sut.Id.Should().NotBeEmpty();
        sut.Clinic.Should().Be(clinicianCategoryBuilder.Clinic);
        sut.ClinicId.Should().Be(clinicianCategoryBuilder.Clinic!.Id);
        sut.Name.Should().Be(clinicianCategoryBuilder.Name);
        sut.Abbreviation.Should().Be(clinicianCategoryBuilder.Abbreviation);
        sut.Color.Should().Be(clinicianCategoryBuilder.Color);
        sut.IsDeleted.Should().BeFalse();
        sut.Clinicians.Should().BeEmpty();
    }
    
    
    /* - - - Method: Delete - - - */
    [Fact]
    public void Delete_WithExistingRooms_ThrowsInvalidOperationException()
    {
        // Arrange
        var sut = TestClinicianCategoryBuilder.Create().Build();
        
        // Act
        var act = () => sut.Delete([TestClinicianBuilder.Create().Build()]);
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
}