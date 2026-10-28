using Application.Common.OutputModels.AppointmentOutputModels;
using Domain.Entities.AppointmentEntities;
using FluentAssertions;
using TestUtils.Builders.AppointmentBuilders;

namespace Application.UnitTests.Tests.Common.OutputModels.AppointmentOutputModels;

public class AppointmentCategoryOverviewOutputModelTests
{
    /* - - - Constructor - - - */
    [Fact]
    public void Constructor_WithValidArguments_SetsAllProperties()
    {
        // Arrange
        var appointmentCategory = TestAppointmentCategoryBuilder.Create().Build();
        
        // Act
        var result = new AppointmentCategoryOverviewOutputModel(appointmentCategory);
        
        // Assert
        result.Id.Should().Be(appointmentCategory.Id);
        result.Name.Should().Be(appointmentCategory.Name);
        result.Abbreviation.Should().Be(appointmentCategory.Abbreviation);
        result.Color.Should().Be(appointmentCategory.Color);
    }
}

public class AppointmentCategoryDetailedOutputModelTests
{
    /* - - - Constructor - - - */
    [Fact]
    public void Constructor_WithValidArguments_SetsAllProperties()
    {
        // Arrange
        var appointmentCategory = TestAppointmentCategoryBuilder.Create().Build();
        var appointments = new List<Appointment> { TestAppointmentBuilder.Create().Build() };
        
        // Act
        var result = new AppointmentCategoryDetailedOutputModel(
            appointmentCategory,
            appointments);
        
        // Assert
        result.Id.Should().Be(appointmentCategory.Id);
        result.Name.Should().Be(appointmentCategory.Name);
        result.Abbreviation.Should().Be(appointmentCategory.Abbreviation);
        result.Color.Should().Be(appointmentCategory.Color);
        result.Appointments.Should().BeEquivalentTo(appointments.Select(appointment => new AppointmentOverviewOutputModel(appointment)));
    }
}