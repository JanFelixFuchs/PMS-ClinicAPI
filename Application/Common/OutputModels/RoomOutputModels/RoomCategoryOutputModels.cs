using Domain.Entities.RoomEntities;

namespace Application.Common.OutputModels.RoomOutputModels;

public class RoomCategoryOverviewOutputModel(RoomCategory roomCategory)
{
    public Guid Id { get; init; } = roomCategory.Id;
    public string Name { get; init; } = roomCategory.Name;
    public string Abbreviation { get; init; } = roomCategory.Abbreviation;
    public string Color { get; init; } = roomCategory.Color;
}

public class RoomCategoryDetailedOutputModel(
    RoomCategory roomCategory, 
    ICollection<Room> rooms) 
    : RoomCategoryOverviewOutputModel(roomCategory)
{
    public ICollection<RoomOverviewOutputModel> Rooms { get; init; } = rooms.Select(room => new RoomOverviewOutputModel(room)).ToList();
}