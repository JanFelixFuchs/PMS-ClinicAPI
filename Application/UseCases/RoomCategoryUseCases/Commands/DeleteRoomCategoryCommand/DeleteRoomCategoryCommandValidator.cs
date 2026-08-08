using Application.Common.Behaviours.ValidationBehaviour.Rules;
using FluentValidation;

namespace Application.UseCases.RoomCategoryUseCases.Commands.DeleteRoomCategoryCommand;

public class DeleteRoomCategoryCommandValidator : AbstractValidator<DeleteRoomCategoryCommand>
{
    public DeleteRoomCategoryCommandValidator()
    {
        RuleFor(command => command.Id).ValidRequiredGuid();
    }
}