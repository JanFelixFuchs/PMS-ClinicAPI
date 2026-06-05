using System.ComponentModel.DataAnnotations;

namespace PMS_ClinicAPI.Common.InputModels.RoomInputModels;

public record CreateRoomCategoryInputModel(
    [Required] string Name,
    [Required] string Abbreviation,
    [Required] string Color,
    [Required] ICollection<Guid> RoomIds);
    
public record UpdateRoomCategoryInputModel(
    [Required] string Name,
    [Required] string Abbreviation,
    [Required] string Color,
    [Required] ICollection<Guid> RoomIds);