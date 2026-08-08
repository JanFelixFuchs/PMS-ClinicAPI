using Application.Common.Behaviours.ValidationBehaviour.Rules;
using FluentValidation;

namespace Application.UseCases.DeviceCategoryUseCases.Commands.DeleteDeviceCategoryCommand;

public class DeleteDeviceCategoryCommandValidator : AbstractValidator<DeleteDeviceCategoryCommand>
{
    public DeleteDeviceCategoryCommandValidator()
    {
        RuleFor(command => command.Id).ValidRequiredGuid();
    }
}