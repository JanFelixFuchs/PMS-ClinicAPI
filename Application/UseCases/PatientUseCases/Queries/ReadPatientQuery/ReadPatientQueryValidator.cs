using Application.Common.Behaviours.ValidationBehaviour.Rules;
using FluentValidation;

namespace Application.UseCases.PatientUseCases.Queries.ReadPatientQuery;

public class ReadPatientQueryValidator : AbstractValidator<ReadPatientQuery>
{
    public ReadPatientQueryValidator()
    {
        RuleFor(query => query.Id).ValidRequiredGuid();
    }
}