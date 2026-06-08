using System.ComponentModel.DataAnnotations;

namespace PMS_ClinicAPI.Common.InputModels.AppointmentInputModels;

public record UpdateAppointmentProtocolInputModel(
    string? Symptoms,
    string? Diagnosis,
    string? Treatment,
    string? Remarks,
    [Required] Guid? ClinicianId,
    [Required] Guid? RoomId,
    [Required] ICollection<Guid> DeviceIds);