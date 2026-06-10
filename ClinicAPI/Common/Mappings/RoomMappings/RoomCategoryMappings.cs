using Application.UseCases.RoomCategoryUseCases.Commands.CreateRoomCategoryCommand;
using Application.UseCases.RoomCategoryUseCases.Commands.UpdateRoomCategoryCommand;
using PMS_ClinicAPI.Common.InputModels.RoomInputModels;

namespace PMS_ClinicAPI.Common.Mappings.RoomMappings;

public static class RoomCategoryMappings
{
    public static CreateRoomCategoryCommand ToCreateRoomCategoryCommand(this CreateRoomCategoryInputModel createRoomCategoryInputModel)
    {
        return new CreateRoomCategoryCommand(
            createRoomCategoryInputModel.Name,
            createRoomCategoryInputModel.Abbreviation, 
            createRoomCategoryInputModel.Color,
            createRoomCategoryInputModel.RoomIds);
    }
    
    public static UpdateRoomCategoryCommand ToUpdateRoomCategoryCommand(
        this UpdateRoomCategoryInputModel updateRoomCategoryInputModel,
        Guid id)
    {
        return new UpdateRoomCategoryCommand(
            id,
            updateRoomCategoryInputModel.Name,
            updateRoomCategoryInputModel.Abbreviation,
            updateRoomCategoryInputModel.Color,
            updateRoomCategoryInputModel.RoomIds);
    }
}