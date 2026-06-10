using Application.UseCases.RoomUseCases.Commands.CreateRoomCommand;
using Application.UseCases.RoomUseCases.Commands.UpdateRoomCommand;
using PMS_ClinicAPI.Common.InputModels.RoomInputModels;

namespace PMS_ClinicAPI.Common.Mappings.RoomMappings;

public static class RoomMappings
{
    public static CreateRoomCommand ToCreateRoomCommand(this CreateRoomInputModel createRoomInputModel)
    {
        return new CreateRoomCommand(
            createRoomInputModel.Name,
            createRoomInputModel.Abbreviation,
            createRoomInputModel.RoomCategoryIds,
            createRoomInputModel.RoomNumber,
            createRoomInputModel.Floor,
            createRoomInputModel.Building);
    }

    public static UpdateRoomCommand ToUpdateRoomCommand(
        this UpdateRoomInputModel updateRoomInputModel,
        Guid id)
    {
        return new UpdateRoomCommand(
            id,
            updateRoomInputModel.Name,
            updateRoomInputModel.Abbreviation,
            updateRoomInputModel.RoomCategoryIds,
            updateRoomInputModel.RoomNumber,
            updateRoomInputModel.Floor,
            updateRoomInputModel.Building);   
    }
}