using Application.Common.Behaviours.ValidationBehaviour.Rules;
using FluentValidation;

namespace Application.UseCases.AppointmentCategoryUseCases.Commands.DeleteAppointmentCategoryCommand;

public class DeleteAppointmentCategoryCommandValidator : AbstractValidator<DeleteAppointmentCategoryCommand>
{
    public DeleteAppointmentCategoryCommandValidator()
    {
        RuleFor(command => command.Id).ValidRequiredGuid();
    }
}