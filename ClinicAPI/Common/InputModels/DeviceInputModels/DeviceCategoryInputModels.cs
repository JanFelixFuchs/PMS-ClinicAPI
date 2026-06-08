using System.ComponentModel.DataAnnotations;

namespace PMS_ClinicAPI.Common.InputModels.DeviceInputModels;

public record CreateDeviceCategoryInputModel(
    [Required] string Name,
    [Required] string Abbreviation,
    [Required] string Color,
    [Required] ICollection<Guid> DeviceIds);

public record UpdateDeviceCategoryInputModel(
    [Required] string Name,
    [Required] string Abbreviation,
    [Required] string Color,
    [Required] ICollection<Guid> DeviceIds);