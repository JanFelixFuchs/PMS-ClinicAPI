using Application.Common.OutputModels.AppointmentOutputModels;
using Domain.Entities.AppointmentEntities;
using Domain.Entities.RoomEntities;

namespace Application.Common.OutputModels.RoomOutputModels;

public class RoomOverviewOutputModel(Room room)
{
    public Guid Id { get; init; } = room.Id;
    public string Name { get; init; } = room.Name;
    public string Abbreviation { get; init; } = room.Abbreviation;
    public bool IsArchived { get; init; } = room.IsArchived;
    public string? RoomNumber { get; init; } = room.RoomNumber;
    public string? Floor { get; init; } = room.Floor;
    public string? Building { get; init; } = room.Building;
}

public class RoomDetailedOutputModel(
    Room room, 
    ICollection<RoomCategory> roomCategories, 
    ICollection<Appointment> appointments,
    ICollection<AppointmentProtocol> appointmentProtocols) 
    : RoomOverviewOutputModel(room)
{
    public ICollection<RoomCategoryOverviewOutputModel> RoomCategories { get; init; } = roomCategories.Select(roomCategory => new RoomCategoryOverviewOutputModel(roomCategory)).ToList();
    public ICollection<AppointmentOverviewOutputModel> Appointments { get; init; } = appointments.Select(appointment => new AppointmentOverviewOutputModel(appointment)).ToList();
    public ICollection<AppointmentProtocolOverviewOutputModel> AppointmentProtocols { get; init; } = appointmentProtocols.Select(appointmentProtocol => new AppointmentProtocolOverviewOutputModel(appointmentProtocol)).ToList();
}