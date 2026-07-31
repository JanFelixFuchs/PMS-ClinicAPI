using Application.Common.Behaviours.Validation.Rules;
using Domain.Commons.Utils.Constants;
using FluentValidation;

namespace Application.UseCases.RoomCategoryUseCases.Commands.UpdateRoomCategoryCommand;

public class UpdateRoomCategoryCommandValidator : AbstractValidator<UpdateRoomCategoryCommand>
{
    public UpdateRoomCategoryCommandValidator()
    {
        RuleFor(command => command.Id).ValidRequiredGuid();

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