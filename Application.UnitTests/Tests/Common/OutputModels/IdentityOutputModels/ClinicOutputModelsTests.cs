using Application.Common.OutputModels.IdentityOutputModels;
using Application.Common.OutputModels.SharedOutputModels;
using FluentAssertions;
using TestUtils.Builders.AuthBuilders;

namespace Application.UnitTests.Tests.Common.OutputModels.IdentityOutputModels;


public class ClinicOverviewOutputModelTests
{
    /* - - - Constructor - - - */
    [Fact]
    public void Constructor_WithValidArguments_SetsAllProperties()
    {
        // Arrange
        var clinic = TestClinicBuilder.Create().Build();
        
        // Act
        var result = new ClinicOutputModel(clinic);
        
        // Assert
        result.Name.Should().Be(clinic.Name);
        result.Abbreviation.Should().Be(clinic.Abbreviation);
        result.Owner.Should().Be(clinic.Owner);
        result.MedicalField.Should().Be(clinic.MedicalField);
        result.Address.Should().BeEquivalentTo(new AddressOutputModel(clinic.Address));
        result.ContactInformation.Should().BeEquivalentTo(new ContactInformationOutputModel(clinic.ContactInformation));
    }
}