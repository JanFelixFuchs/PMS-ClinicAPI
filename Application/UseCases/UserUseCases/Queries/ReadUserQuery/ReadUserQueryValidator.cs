using Application.Common.Behaviours.ValidationBehaviour.Rules;
using FluentValidation;

namespace Application.UseCases.UserUseCases.Queries.ReadUserQuery;

public class ReadUserQueryValidator : AbstractValidator<ReadUserQuery>
{
    public ReadUserQueryValidator()
    {
        RuleFor(query => query.Id).ValidRequiredGuid();
    }
}