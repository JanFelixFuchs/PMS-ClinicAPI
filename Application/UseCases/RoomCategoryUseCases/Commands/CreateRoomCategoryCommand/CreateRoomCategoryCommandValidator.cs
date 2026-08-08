using Application.Common.Behaviours.ValidationBehaviour.Rules;
using Domain.Common.Utils.Constants;
using FluentValidation;

namespace Application.UseCases.RoomCategoryUseCases.Commands.CreateRoomCategoryCommand;

public class CreateRoomCategoryCommandValidator : AbstractValidator<CreateRoomCategoryCommand>
{
    public CreateRoomCategoryCommandValidator()
    {
        RuleFor(command => command.Name)
            .ValidRequiredString()
            .ValidRequiredMaximumStringLength(Lengths.CategoryName);
        
        RuleFor(command => command.Abbreviation)
            .ValidRequiredString()
            .ValidRequiredMaximumStringLength(Lengths.Abbreviation);
        
        RuleFor(command => command.Color)
            .ValidRequiredString()
            .ValidRequiredRegex(RegexPatterns.Color);
        
        RuleFor(command => command.RoomIds)
            .ValidRequiredCollection()
            .ValidRequiredDuplicateFreeCollection();
        RuleForEach(command => command.RoomIds).ValidRequiredGuid();
    }
}