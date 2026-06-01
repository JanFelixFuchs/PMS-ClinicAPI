using Application.Common.OutputModels.ClinicianOutputModels;
using Application.Common.OutputModels.DeviceOutputModels;
using Application.Common.OutputModels.PatientOutputModels;
using Domain.Commons.Enums;
using Domain.Entities.AppointmentEntities;
using Domain.Entities.ClinicianEntities;
using Domain.Entities.DeviceEntities;
using Domain.Entities.PatientEntities;

namespace Application.Common.OutputModels.AppointmentOutputModels;

public class ResultOverviewOutputModel(Result result)
{
    public Guid Id { get; init; } = result.Id;
    public string Title { get; init; } = result.Title;
    public DateTime DateOfCreation { get; init; } = result.DateOfCreation;
}

public class ResultDetailedOutputModel(
    Result result,
    Patient patient,
    Clinician clinician,
    Device? device) 
    : ResultOverviewOutputModel(result)
{
    public AppendixContentType AppendixContentType { get; init; } = result.AppendixContentType;
    public byte[] Appendix { get; init; } = result.Appendix;
    public string? Remarks { get; init; } = result.Remarks;
    public PatientOverviewOutputModel Patient { get; init; } = new(patient);
    public ClinicianOverviewOutputModel Clinician { get; init; } = new(clinician);
    public DeviceOverviewOutputModel? Device { get; init; } = device != null ? new DeviceOverviewOutputModel(device) : null;
}