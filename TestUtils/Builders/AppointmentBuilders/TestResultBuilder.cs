using Domain.Entities.AppointmentEntities;
using Domain.Entities.ClinicianEntities;
using Domain.Entities.DeviceEntities;
using Domain.Entities.IdentityEntities;
using Domain.Entities.PatientEntities;
using TestUtils.Builders.AuthBuilders;
using TestUtils.Builders.ClinicianBuilders;
using TestUtils.Builders.DeviceBuilders;
using TestUtils.Builders.PatientBuilders;
using TestUtils.Constants;

namespace TestUtils.Builders.AppointmentBuilders;

public class TestResultBuilder
{
    // Properties
    public Clinic? Clinic { get; private set; }
    public string? Title { get; private set; }
    public DateTime DateOfCreation { get; private set; }
    public byte[]? Appendix { get; private set; }
    public string? Remarks { get; private set; }
    public Patient? Patient { get; private set; }
    public Clinician? Clinician { get; private set; }
    public Device? Device { get; private set; }
    public readonly DateTime CreationDateTime;
    
    // Constructor
    private TestResultBuilder(
        Clinic defaultClinic, 
        DateTime creationDateTime)
    {
        // Setting default values
        Clinic = defaultClinic;
        Title = "test-title";
        DateOfCreation = creationDateTime;
        Appendix = [0x25, 0x50, 0x44, 0x46, 0x00, 0x01, 0x02, 0x03, 0x04];
        Remarks = "test-remarks";
        Patient = TestPatientBuilder.Create(defaultClinic).Build();
        Clinician = TestClinicianBuilder.Create(defaultClinic).Build();
        Device = TestDeviceBuilder.Create(defaultClinic).Build();
        CreationDateTime = creationDateTime;
    }
    
    
    /* - - - Factory methods - - - */
    public static TestResultBuilder Create(
        Clinic? defaultClinic = null,
        DateTime? currentDateTime = null)
    {
        return new TestResultBuilder(
            defaultClinic ?? TestClinicBuilder.Create().Build(),
            currentDateTime ?? TestConstants.DefaultCurrentDateTime);
    }

    public Result Build()
    {
        return new Result(
            Clinic!,
            Title!,
            DateOfCreation,
            Appendix!,
            Remarks,
            Patient!,
            Clinician!,
            Device,
            CreationDateTime);
    }
    
    
    /* - - - Override methods - - - */
    public TestResultBuilder WithClinic(Clinic? clinic)
    {
        Clinic = clinic;
        return this;
    }

    public TestResultBuilder WithTitle(string? title)
    {
        Title = title;
        return this;
    }

    public TestResultBuilder WithDateOfCreation(DateTime dateOfCreation)
    {
        DateOfCreation = dateOfCreation;
        return this;
    }

    public TestResultBuilder WithAppendix(byte[]? appendix)
    {
        Appendix = appendix;
        return this;
    }

    public TestResultBuilder WithRemarks(string? remarks)
    {
        Remarks = remarks;
        return this;
    }

    public TestResultBuilder WithPatient(Patient? patient)
    {
        Patient = patient;
        return this;
    }

    public TestResultBuilder WithClinician(Clinician? clinician)
    {
        Clinician = clinician;
        return this;
    }

    public TestResultBuilder WithDevice(Device? device)
    {
        Device = device;
        return this;
    }
}