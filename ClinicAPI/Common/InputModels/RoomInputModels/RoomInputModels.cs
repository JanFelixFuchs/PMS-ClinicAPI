using System.ComponentModel.DataAnnotations;

namespace PMS_ClinicAPI.Common.InputModels.RoomInputModels;

public record CreateRoomInputModel(
    [Required] string Name,
    [Required] string Abbreviation,
    [Required] ICollection<Guid> RoomCategoryIds,
    string? RoomNumber,
    string? Floor,
    string? Building);
    
public record UpdateRoomInputModel(
    [Required] string Name,
    [Required] string Abbreviation,
    [Required] ICollection<Guid> RoomCategoryIds,
    string? RoomNumber,
    string? Floor,
    string? Building);