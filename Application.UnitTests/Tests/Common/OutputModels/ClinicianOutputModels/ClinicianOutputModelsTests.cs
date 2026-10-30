using Application.Common.OutputModels.AppointmentOutputModels;
using Application.Common.OutputModels.ClinicianOutputModels;
using Domain.Entities.AppointmentEntities;
using Domain.Entities.ClinicianEntities;
using FluentAssertions;
using TestUtils.Builders.AppointmentBuilders;
using TestUtils.Builders.ClinicianBuilders;

namespace Application.UnitTests.Tests.Common.OutputModels.ClinicianOutputModels;

public class ClinicianOverviewOutputModelTests
{
    /* - - - Constructor - - - */
    [Fact]
    public void Constructor_WithValidArguments_SetsAllProperties()
    {
        // Arrange
        var clinician = TestClinicianBuilder.Create().Build();
        
        // Act
        var result = new ClinicianOverviewOutputModel(clinician);
        
        // Assert
        result.Id.Should().Be(clinician.Id);
        result.FirstName.Should().Be(clinician.FirstName);
        result.LastName.Should().Be(clinician.LastName);
        result.IsArchived.Should().Be(clinician.IsArchived);
    }
}

public class ClinicianDetailedOutputModelTests
{
    /* - - - Constructor - - - */
    [Fact]
    public void Constructor_WithValidArguments_SetsAllProperties()
    {
        // Arrange
        var clinician = TestClinicianBuilder.Create().Build();
        var clinicianCategories = new List<ClinicianCategory> { TestClinicianCategoryBuilder.Create().Build() };
        var appointments = new List<Appointment> { TestAppointmentBuilder.Create().Build() };
        var appointmentProtocols = new List<AppointmentProtocol> { TestAppointmentProtocolBuilder.Create().Build() };
        var results = new List<Result> { TestResultBuilder.Create().Build() };
        
        // Act
        var result = new ClinicianDetailedOutputModel(
            clinician,
            clinicianCategories,
            appointments,
            appointmentProtocols,
            results);
        
        // Assert
        result.Id.Should().Be(clinician.Id);
        result.FirstName.Should().Be(clinician.FirstName);
        result.LastName.Should().Be(clinician.LastName);
        result.IsArchived.Should().Be(clinician.IsArchived);
        result.ClinicianCategories.Should().BeEquivalentTo(clinicianCategories.Select(clinicianCategory => new ClinicianCategoryOverviewOutputModel(clinicianCategory)));
        result.Appointments.Should().BeEquivalentTo(appointments.Select(appointment => new AppointmentOverviewOutputModel(appointment)));
        result.AppointmentProtocols.Should().BeEquivalentTo(appointmentProtocols.Select(appointmentProtocol => new AppointmentProtocolOverviewOutputModel(appointmentProtocol)));
        result.Results.Should().BeEquivalentTo(results.Select(resultValue => new ResultOverviewOutputModel(resultValue)));
    }
}