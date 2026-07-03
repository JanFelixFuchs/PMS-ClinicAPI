using Application.Common.Behaviours.Validation.Rules;
using FluentValidation;

namespace Application.UseCases.ClinicianUseCases.Queries.ReadClinicianQuery;

public class ReadClinicianQueryValidator : AbstractValidator<ReadClinicianQuery>
{
    public ReadClinicianQueryValidator()
    {
        RuleFor(query => query.Id).ValidRequiredGuid();
    }
}