using Application.Common.Behaviours.Validation.Rules;
using FluentValidation;

namespace Application.UseCases.RoomUseCases.Commands.DeleteRoomCommand;

public class DeleteRoomCommandValidator : AbstractValidator<DeleteRoomCommand>
{
    public DeleteRoomCommandValidator()
    {
        RuleFor(command => command.Id).ValidRequiredGuid();
    }
}