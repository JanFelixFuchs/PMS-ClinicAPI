using Application.Common.Behaviours.Validation.Rules;
using FluentValidation;

namespace Application.UseCases.DeviceUseCases.Queries.ReadDeviceQuery;

public class ReadDeviceQueryValidator : AbstractValidator<ReadDeviceQuery>
{
    public ReadDeviceQueryValidator()
    {
        RuleFor(query => query.Id).ValidRequiredGuid();
    }
}