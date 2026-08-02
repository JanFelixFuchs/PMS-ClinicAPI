using Application.Common.Behaviours.Validation.Rules;
using FluentValidation;

namespace Application.UseCases.RoomUseCases.Queries.ReadRoomQuery;

public class ReadRoomQueryValidator : AbstractValidator<ReadRoomQuery>
{
    public ReadRoomQueryValidator()
    {
        RuleFor(query => query.Id).ValidRequiredGuid();
    }
}