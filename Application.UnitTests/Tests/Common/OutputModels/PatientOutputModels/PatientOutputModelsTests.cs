using Application.Common.OutputModels.AppointmentOutputModels;
using Application.Common.OutputModels.PatientOutputModels;
using Application.Common.OutputModels.SharedOutputModels;
using Domain.Entities.AppointmentEntities;
using FluentAssertions;
using TestUtils.Builders.AppointmentBuilders;
using TestUtils.Builders.PatientBuilders;

namespace Application.UnitTests.Tests.Common.OutputModels.PatientOutputModels;

public class PatientOverviewOutputModelTests
{
    /* - - - Constructor - - - */
    [Fact]
    public void Constructor_WithValidArguments_SetsAllProperties()
    {
        // Arrange
        var patient = TestPatientBuilder.Create().Build();
        
        // Act
        var result = new PatientOverviewOutputModel(patient);
        
        // Assert
        result.Id.Should().Be(patient.Id);
        result.FirstName.Should().Be(patient.FirstName);
        result.LastName.Should().Be(patient.LastName);
        result.DateOfBirth.Should().Be(patient.DateOfBirth);
        result.Gender.Should().Be(patient.Gender);
        result.InsuranceStatus.Should().Be(patient.InsuranceStatus);
        result.IsArchived.Should().Be(patient.IsArchived);
    }
}

public class PatientDetailedOutputModelTests
{
    /* - - - Constructor - - - */
    [Fact]
    public void Constructor_WithValidArguments_SetsAllProperties()
    {
        // Arrange
        var patient = TestPatientBuilder.Create().Build();
        var appointments = new List<Appointment> { TestAppointmentBuilder.Create().Build() };
        var appointmentProtocols = new List<AppointmentProtocol> { TestAppointmentProtocolBuilder.Create().Build() };
        var results = new List<Result> { TestResultBuilder.Create().Build() };
        
        // Act
        var result = new PatientDetailedOutputModel(
            patient,
            appointments,
            appointmentProtocols,
            results);
        
        // Assert
        result.Id.Should().Be(patient.Id);
        result.FirstName.Should().Be(patient.FirstName);
        result.LastName.Should().Be(patient.LastName);
        result.DateOfBirth.Should().Be(patient.DateOfBirth);
        result.Gender.Should().Be(patient.Gender);
        result.InsuranceStatus.Should().Be(patient.InsuranceStatus);
        result.IsArchived.Should().Be(patient.IsArchived);
        result.DateOfCreation.Should().Be(patient.DateOfCreation);
        result.Address.Should().BeEquivalentTo(new AddressOutputModel(patient.Address));
        result.ContactInformation.Should().BeEquivalentTo(new ContactInformationOutputModel(patient.ContactInformation));
        result.Allergies.Should().Be(patient.Allergies);
        result.Remarks.Should().Be(patient.Remarks);
        result.Appointments.Should().BeEquivalentTo(appointments.Select(appointment => new AppointmentOverviewOutputModel(appointment)));
        result.AppointmentProtocols.Should().BeEquivalentTo(appointmentProtocols.Select(appointmentProtocol => new AppointmentProtocolOverviewOutputModel(appointmentProtocol)));
        result.Results.Should().BeEquivalentTo(results.Select(resultValue => new ResultOverviewOutputModel(resultValue)));
    }
}