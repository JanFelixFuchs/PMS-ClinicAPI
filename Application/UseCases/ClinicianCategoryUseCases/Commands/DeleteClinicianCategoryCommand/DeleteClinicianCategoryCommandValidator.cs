using Application.Common.Behaviours.Validation.Rules;
using FluentValidation;

namespace Application.UseCases.ClinicianCategoryUseCases.Commands.DeleteClinicianCategoryCommand;

public class DeleteClinicianCategoryCommandValidator : AbstractValidator<DeleteClinicianCategoryCommand>
{
    public DeleteClinicianCategoryCommandValidator()
    {
        RuleFor(command => command.Id).ValidRequiredGuid();
    }
}