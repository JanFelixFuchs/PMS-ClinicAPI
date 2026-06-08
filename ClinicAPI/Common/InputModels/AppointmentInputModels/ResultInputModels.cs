using System.ComponentModel.DataAnnotations;

namespace PMS_ClinicAPI.Common.InputModels.AppointmentInputModels;

public record CreateResultInputModel(
    [Required] string Title,
    [Required] DateTime? DateOfCreation,
    [Required] byte[] Appendix,
    [Required] Guid? PatientId,
    string? Remarks,
    [Required] Guid? ClinicianId,
    Guid? DeviceId);