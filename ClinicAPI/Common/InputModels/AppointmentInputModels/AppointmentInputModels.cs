using System.ComponentModel.DataAnnotations;

namespace PMS_ClinicAPI.Common.InputModels.AppointmentInputModels;

public record CreateAppointmentInputModel(
    [Required] string Title,
    [Required] DateTime? StartTime,
    [Required] DateTime? EndTime, 
    [Required] ICollection<Guid> AppointmentCategoryIds,
    [Required] Guid? PatientId,
    [Required] Guid? RoomId,
    [Required] ICollection<Guid> DeviceIds,
    [Required] ICollection<Guid> ClinicianIds);
    
public record UpdateAppointmentInputModel(
    [Required] string Title,
    [Required] DateTime? StartTime,
    [Required] DateTime? EndTime, 
    [Required] ICollection<Guid> AppointmentCategoryIds,
    [Required] Guid? PatientId,
    [Required] Guid? RoomId,
    [Required] ICollection<Guid> DeviceIds,
    [Required] ICollection<Guid> ClinicianIds);