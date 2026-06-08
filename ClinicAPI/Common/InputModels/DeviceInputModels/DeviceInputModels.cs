using System.ComponentModel.DataAnnotations;
using Domain.Commons.Enums;

namespace PMS_ClinicAPI.Common.InputModels.DeviceInputModels;

public record CreateDeviceInputModel(
    [Required] string Name,
    [Required] string Abbreviation,
    [Required] string SerialNumber,
    [Required] DeviceStatus? Status,
    [Required] string Producer,
    [Required] ICollection<Guid> DeviceCategoryIds,
    DateTime? DateOfPurchase,
    DateTime? DateOfLastMaintenance);

public record UpdateDeviceInputModel(
    [Required] string Name,
    [Required] string Abbreviation,
    [Required] ICollection<Guid> DeviceCategoryIds,
    DateTime? DateOfLastMaintenance);