using System.ComponentModel.DataAnnotations;

namespace PMS_ClinicAPI.Common.InputModels.ClinicianInputModels;

public record CreateClinicianInputModel(
    [Required] string FirstName,
    [Required] string LastName,
    [Required] ICollection<Guid> ClinicianCategoryIds);
    
public record UpdateClinicianInputModel(
    [Required] string LastName,
    [Required] ICollection<Guid> ClinicianCategoryIds);