using Application.Common.Behaviours.Validation.Rules;
using FluentValidation;

namespace Application.UseCases.RoleUseCases.Queries.ReadRoleQuery;

public class ReadRoleQueryValidator : AbstractValidator<ReadRoleQuery>
{
    public ReadRoleQueryValidator()
    {
        RuleFor(query => query.Id).ValidRequiredGuid();
    }
}