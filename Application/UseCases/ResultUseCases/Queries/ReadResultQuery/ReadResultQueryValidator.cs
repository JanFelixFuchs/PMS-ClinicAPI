using Application.Common.Behaviours.Validation.Rules;
using FluentValidation;

namespace Application.UseCases.ResultUseCases.Queries.ReadResultQuery;

public class ReadResultQueryValidator : AbstractValidator<ReadResultQuery>
{
    public ReadResultQueryValidator()
    {
        RuleFor(query => query.Id).ValidRequiredGuid();
    }
}