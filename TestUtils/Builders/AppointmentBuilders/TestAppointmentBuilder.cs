using Domain.Entities.AppointmentEntities;
using Domain.Entities.ClinicianEntities;
using Domain.Entities.DeviceEntities;
using Domain.Entities.IdentityEntities;
using Domain.Entities.PatientEntities;
using Domain.Entities.RoomEntities;
using TestUtils.Builders.AuthBuilders;
using TestUtils.Builders.ClinicianBuilders;
using TestUtils.Builders.DeviceBuilders;
using TestUtils.Builders.PatientBuilders;
using TestUtils.Builders.RoomBuilders;
using TestUtils.Constants;

namespace TestUtils.Builders.AppointmentBuilders;

public class TestAppointmentBuilder
{
    // Properties
    public Clinic? Clinic { get; private set; }
    public string? Title { get; private set; }
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }
    public ICollection<AppointmentCategory>? AppointmentCategories { get; private set; }
    public Patient? Patient { get; private set; }
    public Room? Room { get; private set; }
    public ICollection<Device>? Devices { get; private set; }
    public ICollection<Clinician>? Clinicians { get; private set; }
    public readonly DateTime CreationDateTime;
    private DateTime _updateDateTime;
    
    // Constructor
    private TestAppointmentBuilder(
        Clinic defaultClinic,
        DateTime creationDateTime)
    {
        // Setting default values
        Clinic = defaultClinic;
        Title = "test-title";
        StartTime = creationDateTime;
        EndTime = creationDateTime.AddMinutes(30);
        AppointmentCategories = new List<AppointmentCategory>{ TestAppointmentCategoryBuilder.Create(defaultClinic).Build() };
        Patient = TestPatientBuilder.Create(defaultClinic).Build();
        Room = TestRoomBuilder.Create(defaultClinic).Build();
        Devices = new List<Device>{ TestDeviceBuilder.Create(defaultClinic).Build() };
        Clinicians = new List<Clinician> { TestClinicianBuilder.Create(defaultClinic).Build() };
        CreationDateTime = creationDateTime;
    }
    
    
    /* - - -  Factory methods - - - */
    public static TestAppointmentBuilder Create(
        Clinic? defaultClinic = null,
        DateTime? currentDateTime = null)
    {
        return new TestAppointmentBuilder(
            defaultClinic ?? TestClinicBuilder.Create().Build(),
            currentDateTime ?? TestConstants.DefaultCurrentDateTime);
    }

    public Appointment Build()
    {
        return new Appointment(
            Clinic!,
            Title!,
            StartTime,
            EndTime,
            AppointmentCategories!,
            Patient!,
            Room!,
            Devices!,
            Clinicians!,
            CreationDateTime);
    }
    
    public TestAppointmentBuilder AsUpdate(DateTime? updateDateTime = null)
    {
        // Setting custom update time if provided
        if (updateDateTime.HasValue)
            _updateDateTime = updateDateTime.Value;
        
        // Setting default values
        Title = "test-updated-title";
        StartTime = _updateDateTime;
        EndTime = _updateDateTime.AddMinutes(30);
        AppointmentCategories = new List<AppointmentCategory>{ TestAppointmentCategoryBuilder.Create(Clinic).Build() };
        Patient = TestPatientBuilder.Create(Clinic).Build();
        Room = TestRoomBuilder.Create(Clinic).Build();
        Devices = new List<Device>{ TestDeviceBuilder.Create(Clinic).Build() };
        Clinicians = new List<Clinician>{ TestClinicianBuilder.Create(Clinic).Build() };

        // Returning instance
        return this;
    }

    public Action Apply(Appointment appointment) =>
        () => appointment.Update(
            Title!,
            StartTime,
            EndTime,
            AppointmentCategories!,
            Patient!,
            Room!,
            Devices!,
            Clinicians!,
            _updateDateTime);
    
    
    /* - - - Override methods - - - */
    public TestAppointmentBuilder WithClinic(Clinic? clinic)
    {
        Clinic = clinic;
        return this;
    }

    public TestAppointmentBuilder WithTitle(string? title)
    {
        Title = title;
        return this;
    }

    public TestAppointmentBuilder WithStartTime(DateTime startTime)
    {
        StartTime = startTime;
        return this;
    }

    public TestAppointmentBuilder WithEndTime(DateTime endTime)
    {
        EndTime = endTime;
        return this;
    }

    public TestAppointmentBuilder WithAppointmentCategories(ICollection<AppointmentCategory>? appointmentCategories)
    {
        AppointmentCategories = appointmentCategories;
        return this;
    }

    public TestAppointmentBuilder WithPatient(Patient? patient)
    {
        Patient = patient;
        return this;
    }

    public TestAppointmentBuilder WithRoom(Room? room)
    {
        Room = room;
        return this;
    }

    public TestAppointmentBuilder WithDevices(ICollection<Device>? devices)
    {
        Devices = devices;
        return this;
    }

    public TestAppointmentBuilder WithClinicians(ICollection<Clinician>? clinicians)
    {
        Clinicians = clinicians;
        return this;
    }
}