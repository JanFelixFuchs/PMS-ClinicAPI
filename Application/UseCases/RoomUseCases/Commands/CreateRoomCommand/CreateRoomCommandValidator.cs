using Application.Common.Behaviours.Validation.Rules;
using Domain.Common.Utils.Constants;
using FluentValidation;

namespace Application.UseCases.RoomUseCases.Commands.CreateRoomCommand;

public class CreateRoomCommandValidator : AbstractValidator<CreateRoomCommand>
{
    public CreateRoomCommandValidator()
    {
        RuleFor(command => command.Name)
            .ValidRequiredString()
            .ValidRequiredMaximumStringLength(Lengths.RoomName);

        RuleFor(command => command.Abbreviation)
            .ValidRequiredString()
            .ValidRequiredMaximumStringLength(Lengths.Abbreviation);
        
        RuleFor(command => command.RoomCategoryIds)
            .ValidRequiredCollection()
            .ValidRequiredDuplicateFreeCollection();
        RuleForEach(command => command.RoomCategoryIds).ValidRequiredGuid();
        
        RuleFor(command => command.RoomNumber)
            .ValidOptionalString()
            .ValidOptionalMaximumStringLength(Lengths.RoomNumber);

        RuleFor(command => command.Floor)
            .ValidOptionalString()
            .ValidOptionalMaximumStringLength(Lengths.Floor);

        RuleFor(command => command.Building)
            .ValidOptionalString()
            .ValidOptionalMaximumStringLength(Lengths.Building);
    }
}