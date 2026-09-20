using Domain.Entities.AppointmentEntities;
using Domain.Entities.ClinicianEntities;
using Domain.Entities.DeviceEntities;
using Domain.Entities.IdentityEntities;
using Domain.Entities.RoomEntities;
using TestUtils.Builders.AuthBuilders;
using TestUtils.Builders.ClinicianBuilders;
using TestUtils.Builders.DeviceBuilders;
using TestUtils.Builders.RoomBuilders;
using TestUtils.Constants;

namespace TestUtils.Builders.AppointmentBuilders;

public class TestAppointmentProtocolBuilder
{
    // Properties
    public Clinic? Clinic { get; private set; }
    public string? Symptoms { get; private set; }
    public string? Diagnosis { get; private set; }
    public string? Treatment { get; private set; }
    public string? Remarks { get; private set; }
    public Appointment? Appointment { get; private set; }
    public Clinician? Clinician { get; private set; }
    public Room? Room { get; private set; }
    public ICollection<Device>? Devices { get; private set; }
    public readonly DateTime CreationDateTime;
    
    // Constructor
    private TestAppointmentProtocolBuilder(
        Clinic defaultClinic,
        DateTime creationDateTime)
    {
        // Generating default values
        var appointment = TestAppointmentBuilder.Create(defaultClinic, creationDateTime).Build();
        appointment.MarkAsAttended(creationDateTime);
        
        // Setting default values
        Clinic = appointment.Clinic;
        Appointment = appointment;
        CreationDateTime = creationDateTime;
    }
    
    
    /* - - - Factory methods - - - */
    public static TestAppointmentProtocolBuilder Create(
        Clinic? defaultClinic = null,
        DateTime? currentDateTime = null)
    {
        return new TestAppointmentProtocolBuilder(
            defaultClinic ?? TestClinicBuilder.Create().Build(),
            currentDateTime ?? TestConstants.DefaultCurrentDateTime);
    }

    public AppointmentProtocol Build()
    {
        return new AppointmentProtocol(
            Clinic!,
            Appointment!,
            CreationDateTime);
    }

    public TestAppointmentProtocolBuilder AsUpdate()
    {
        // Setting default values
        Symptoms = "test-updated-symptoms";
        Diagnosis = "test-updated-diagnosis";
        Treatment = "test-updated-treatment";
        Remarks = "test-updated-remarks";
        Clinician = TestClinicianBuilder.Create(Clinic).Build();
        Room = TestRoomBuilder.Create(Clinic).Build();
        Devices = new List<Device>{  TestDeviceBuilder.Create(Clinic).Build() };
        
        // Returning instance
        return this;
    }

    public Action Apply(AppointmentProtocol appointmentProtocol) =>
        () => appointmentProtocol.Update(
            Symptoms,
            Diagnosis,
            Treatment,
            Remarks,
            Clinician!,
            Room!,
            Devices!);
    
    
    /* - - - Override methods - - - */
    public TestAppointmentProtocolBuilder WithClinic(Clinic? clinic)
    {
        Clinic = clinic;
        return this;
    }

    public TestAppointmentProtocolBuilder WithSymptoms(string? symptoms)
    {
        Symptoms = symptoms;
        return this;
    }
    
    public TestAppointmentProtocolBuilder WithDiagnosis(string? diagnosis)
    {
        Diagnosis = diagnosis;
        return this;
    }
    
    public TestAppointmentProtocolBuilder WithTreatment(string? treatment)
    {
        Treatment = treatment;
        return this;
    }
    
    public TestAppointmentProtocolBuilder WithRemarks(string? remarks)
    {
        Remarks = remarks;
        return this;
    }
    
    public TestAppointmentProtocolBuilder WithAppointment(Appointment? appointment)
    {
        Appointment = appointment;
        return this;
    }

    public TestAppointmentProtocolBuilder WithClinician(Clinician? clinician)
    {
        Clinician = clinician;
        return this;
    }

    public TestAppointmentProtocolBuilder WithRoom(Room? room)
    {
        Room = room;
        return this;
    }

    public TestAppointmentProtocolBuilder WithDevices(ICollection<Device>? devices)
    {
        Devices = devices;
        return this;
    }
}