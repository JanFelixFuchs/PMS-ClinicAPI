using System.ComponentModel.DataAnnotations;

namespace PMS_ClinicAPI.Common.InputModels.AppointmentInputModels;

public record CreateAppointmentCategoryInputModel(
    [Required] string Name,
    [Required] string Abbreviation,
    [Required] string Color);
    
public record UpdateAppointmentCategoryInputModel(
    [Required] string Name,
    [Required] string Abbreviation,
    [Required] string Color);