using Application.Common.Behaviours.Validation.Rules;
using FluentValidation;

namespace Application.UseCases.RoomCategoryUseCases.Queries.ReadRoomCategoryQuery;

public class ReadRoomCategoryQueryValidator : AbstractValidator<ReadRoomCategoryQuery>
{
    public ReadRoomCategoryQueryValidator()
    {
        RuleFor(query => query.Id).ValidRequiredGuid();
    }
}