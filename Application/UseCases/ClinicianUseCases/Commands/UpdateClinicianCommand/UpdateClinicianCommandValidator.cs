using Application.Common.Behaviours.Validation.Rules;
using Domain.Commons.Utils.Constants;
using FluentValidation;

namespace Application.UseCases.ClinicianUseCases.Commands.UpdateClinicianCommand;

public class UpdateClinicianCommandValidator : AbstractValidator<UpdateClinicianCommand>
{
    public UpdateClinicianCommandValidator()
    {
        RuleFor(command=> command.Id).ValidRequiredGuid();

        RuleFor(command => command.LastName)
            .ValidRequiredString()
            .ValidRequiredMaximumStringLength(Lengths.LastName);

        RuleFor(command => command.ClinicianCategoryIds)
            .ValidRequiredCollection()
            .ValidRequiredDuplicateFreeCollection();
        RuleForEach(command => command.ClinicianCategoryIds).ValidRequiredGuid();
    }
}