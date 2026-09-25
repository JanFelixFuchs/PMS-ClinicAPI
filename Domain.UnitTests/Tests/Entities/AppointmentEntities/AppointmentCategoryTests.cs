using FluentAssertions;
using TestUtils.Builders.AppointmentBuilders;
using InvalidOperationException = Domain.Common.Exceptions.InvalidOperationException;

namespace Domain.Tests.Tests.Entities.AppointmentEntities;

public class AppointmentCategoryTests
{
    /* - - - Constructor - - - */
    [Fact]
    public void Constructor_WithValidArguments_SetsAllProperties()
    {
        // Arrange
        var appointmentCategoryBuilder = TestAppointmentCategoryBuilder.Create();
        
        // Act
        var sut = appointmentCategoryBuilder.Build();
        
        // Assert
        sut.Id.Should().NotBeEmpty();
        sut.Clinic.Should().Be(appointmentCategoryBuilder.Clinic);
        sut.ClinicId.Should().Be(appointmentCategoryBuilder.Clinic!.Id);
        sut.Name.Should().Be(appointmentCategoryBuilder.Name);
        sut.Abbreviation.Should().Be(appointmentCategoryBuilder.Abbreviation);
        sut.Color.Should().Be(appointmentCategoryBuilder.Color);
        sut.IsDeleted.Should().BeFalse();
        sut.Appointments.Should().BeEmpty();
    }
    
    
    /* - - - Method: Delete - - - */
    [Fact]
    public void Delete_WithExistingRooms_ThrowsInvalidOperationException()
    {
        // Arrange
        var sut = TestAppointmentCategoryBuilder.Create().Build();
        
        // Act
        var act = () => sut.Delete([TestAppointmentBuilder.Create().Build()]);
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
}