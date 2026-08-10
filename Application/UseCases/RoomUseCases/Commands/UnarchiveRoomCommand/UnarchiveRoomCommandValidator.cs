using Application.Common.Behaviours.ValidationBehaviour.Rules;
using FluentValidation;

namespace Application.UseCases.RoomUseCases.Commands.UnarchiveRoomCommand;

public class UnarchiveRoomCommandValidator : AbstractValidator<UnarchiveRoomCommand>
{
    public UnarchiveRoomCommandValidator()
    {
        RuleFor(command => command.Id).ValidRequiredGuid();
    }
}