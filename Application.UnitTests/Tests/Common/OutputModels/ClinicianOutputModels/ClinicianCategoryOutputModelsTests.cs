using Application.Common.OutputModels.ClinicianOutputModels;
using Domain.Entities.ClinicianEntities;
using FluentAssertions;
using TestUtils.Builders.ClinicianBuilders;

namespace Application.UnitTests.Tests.Common.OutputModels.ClinicianOutputModels;

public class ClinicianCategoryOverviewOutputModelTests
{
    /* - - - Constructor - - - */
    [Fact]
    public void Constructor_WithValidArguments_SetsAllProperties()
    {
        // Arrange
        var clinicianCategory = TestClinicianCategoryBuilder.Create().Build();
        
        // Act
        var result = new ClinicianCategoryOverviewOutputModel(clinicianCategory);
        
        // Assert
        result.Id.Should().Be(clinicianCategory.Id);
        result.Name.Should().Be(clinicianCategory.Name);
        result.Abbreviation.Should().Be(clinicianCategory.Abbreviation);
        result.Color.Should().Be(clinicianCategory.Color);
    }
}

public class ClinicianCategoryDetailedOutputModelTests
{
    /* - - - Constructor - - - */
    [Fact]
    public void Constructor_WithValidArguments_SetsAllProperties()
    {
        // Arrange
        var clinicianCategory = TestClinicianCategoryBuilder.Create().Build();
        var clinicians = new List<Clinician> { TestClinicianBuilder.Create().Build() };
        
        // Act
        var result = new ClinicianCategoryDetailedOutputModel(
            clinicianCategory,
            clinicians);
        
        // Assert
        result.Id.Should().Be(clinicianCategory.Id);
        result.Name.Should().Be(clinicianCategory.Name);
        result.Abbreviation.Should().Be(clinicianCategory.Abbreviation);
        result.Color.Should().Be(clinicianCategory.Color);
        result.Clinicians.Should().BeEquivalentTo(clinicians.Select(clinician => new ClinicianOverviewOutputModel(clinician)));
    }
}