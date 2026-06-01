using Application.Common.OutputModels.AppointmentOutputModels;
using Application.Common.OutputModels.SharedOutputModels;
using Domain.Commons.Enums;
using Domain.Entities.AppointmentEntities;
using Domain.Entities.PatientEntities;

namespace Application.Common.OutputModels.PatientOutputModels;

public class PatientOverviewOutputModel(Patient patient)
{
    public Guid Id  { get; init; } = patient.Id;
    public string FirstName { get; init; } = patient.FirstName;
    public string LastName { get; init; } = patient.LastName;
    public DateTime DateOfBirth { get; init; } = patient.DateOfBirth;
    public Gender Gender { get; init; } = patient.Gender;
    public InsuranceStatus InsuranceStatus { get; init; } = patient.InsuranceStatus;
    public bool IsArchived { get; init; } = patient.IsArchived;
}

public class PatientDetailedOutputModel(
    Patient patient,
    ICollection<Appointment> appointments, 
    ICollection<AppointmentProtocol> appointmentProtocols,
    ICollection<Result> results) 
    : PatientOverviewOutputModel(patient)
{
    public DateTime DateOfCreation { get; init; } = patient.DateOfCreation;
    public AddressOutputModel Address { get; init; } = new(patient.Address);
    public ContactInformationOutputModel ContactInformation { get; init; } = new(patient.ContactInformation);
    public string? Allergies { get; init; } = patient.Allergies;
    public string? Remarks { get; init; } = patient.Remarks;
    public ICollection<AppointmentOverviewOutputModel> Appointments { get; init; } = appointments.Select(appointment => new AppointmentOverviewOutputModel(appointment)).ToList();
    public ICollection<AppointmentProtocolOverviewOutputModel> AppointmentProtocols { get; init; } = appointmentProtocols.Select(appointmentProtocol => new AppointmentProtocolOverviewOutputModel(appointmentProtocol)).ToList();
    public ICollection<ResultOverviewOutputModel> Results { get; init; } = results.Select(result => new ResultOverviewOutputModel(result)).ToList();
}