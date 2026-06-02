using Application.Common.OutputModels.AppointmentOutputModels;
using Domain.Entities.AppointmentEntities;
using Domain.Entities.ClinicianEntities;

namespace Application.Common.OutputModels.ClinicianOutputModels;

public class ClinicianOverviewOutputModel(Clinician clinician)
{
    public Guid Id { get; set; } = clinician.Id;
    public string FirstName { get; init; } = clinician.FirstName;
    public string LastName { get; init; } = clinician.LastName;
    public bool IsArchived { get; init; } = clinician.IsArchived;
}

public class ClinicianDetailedOutputModel(
    Clinician clinician,
    ICollection<ClinicianCategory> clinicianCategories,
    ICollection<Appointment> appointments,
    ICollection<AppointmentProtocol> appointmentProtocols,
    ICollection<Result> results) 
    : ClinicianOverviewOutputModel(clinician)
{
    public ICollection<ClinicianCategoryOverviewOutputModel> ClinicianCategories { get; init; } = clinicianCategories.Select(clinicianCategory => new ClinicianCategoryOverviewOutputModel(clinicianCategory)).ToList();
    public ICollection<AppointmentOverviewOutputModel> Appointments { get; init; } = appointments.Select(appointment => new AppointmentOverviewOutputModel(appointment)).ToList();
    public ICollection<AppointmentProtocolOverviewOutputModel> AppointmentProtocols { get; init; } = appointmentProtocols.Select(appointmentProtocol => new AppointmentProtocolOverviewOutputModel((appointmentProtocol))).ToList();
    public ICollection<ResultOverviewOutputModel> Results { get; init; } = results.Select(result => new ResultOverviewOutputModel(result)).ToList();
}