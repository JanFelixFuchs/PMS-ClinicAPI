using Application.Common.Behaviours.Validation.Rules;
using FluentValidation;

namespace Application.UseCases.RoomUseCases.Commands.ArchiveRoomCommand;

public class ArchiveRoomCommandValidator : AbstractValidator<ArchiveRoomCommand>
{
    public ArchiveRoomCommandValidator()
    {
        RuleFor(command => command.Id).ValidRequiredGuid();
    }
}