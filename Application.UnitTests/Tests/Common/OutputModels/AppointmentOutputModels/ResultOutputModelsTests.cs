using Application.Common.OutputModels.AppointmentOutputModels;
using FluentAssertions;
using TestUtils.Builders.AppointmentBuilders;

namespace Application.UnitTests.Tests.Common.OutputModels.AppointmentOutputModels;

public class ResultOverviewOutputModelTests
{
    /* - - - Constructor - - - */
    [Fact]
    public void Constructor_WithValidArguments_SetsAllProperties()
    {
        // Arrange
        var resultValue = TestResultBuilder.Create().Build();
        
        // Act
        var result = new ResultOverviewOutputModel(resultValue);
        
        // Assert
        result.Id.Should().Be(result.Id);
        result.Title.Should().Be(result.Title);
        result.DateOfCreation.Should().Be(result.DateOfCreation);
    }
}

public class ResultDetailedOutputModelTests
{
    /* - - - Constructor - - - */
    [Fact]
    public void Constructor_WithValidArguments_SetsAllProperties()
    {
        // Arrange
        var resultValue = TestResultBuilder.Create().Build();
        
        // Act
        var result = new ResultDetailedOutputModel(
            resultValue,
            resultValue.Patient,
            resultValue.Clinician,
            resultValue.Device);
        
        // Assert
        result.Id.Should().Be(resultValue.Id);
        result.Title.Should().Be(resultValue.Title);
        result.DateOfCreation.Should().Be(resultValue.DateOfCreation);
        result.AppendixContentType.Should().Be(resultValue.AppendixContentType);
        result.Appendix.Should().BeEquivalentTo(resultValue.Appendix);
        result.Remarks.Should().Be(resultValue.Remarks);
        result.Patient.Id.Should().Be(resultValue.Patient.Id);
        result.Clinician.Id.Should().Be(resultValue.Clinician.Id);
        result.Device!.Id.Should().Be(resultValue.Device!.Id);
    }
}