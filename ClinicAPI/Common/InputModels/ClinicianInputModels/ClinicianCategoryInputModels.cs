using System.ComponentModel.DataAnnotations;

namespace PMS_ClinicAPI.Common.InputModels.ClinicianInputModels;

public record CreateClinicianCategoryInputModel(
    [Required] string Name,
    [Required] string Abbreviation,
    [Required] string Color,
    [Required] ICollection<Guid> ClinicianIds);
    
public record UpdateClinicianCategoryInputModel(
    [Required] string Name,
    [Required] string Abbreviation,
    [Required] string Color,
    [Required] ICollection<Guid> ClinicianIds);