using Application.Common.Behaviours.Validation.Rules;
using Domain.Commons.Utils.Constants;
using FluentValidation;

namespace Application.UseCases.ClinicianUseCases.Commands.CreateClinicianCommand;

public class CreateClinicianCommandValidator : AbstractValidator<CreateClinicianCommand>
{
    public CreateClinicianCommandValidator()
    {
        RuleFor(command => command.FirstName)
            .ValidRequiredString()
            .ValidRequiredMaximumStringLength(Lengths.FirstName);
            
        RuleFor(command => command.LastName)
            .ValidRequiredString()
            .ValidRequiredMaximumStringLength(Lengths.LastName);

        RuleFor(command => command.ClinicianCategoryIds)
            .ValidRequiredCollection()
            .ValidRequiredDuplicateFreeCollection();
        RuleForEach(command => command.ClinicianCategoryIds).ValidRequiredGuid();
    }
}