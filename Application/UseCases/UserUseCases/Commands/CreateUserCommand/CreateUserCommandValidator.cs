using Application.Common.Behaviours.ValidationBehaviour.Rules;
using Domain.Common.Utils.Constants;
using FluentValidation;

namespace Application.UseCases.UserUseCases.Commands.CreateUserCommand;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(command => command.Username)
            .ValidRequiredString()
            .ValidRequiredRegex(RegexPatterns.Username);
        
        RuleFor(command => command.Password)
            .ValidRequiredString()
            .ValidRequiredRegex(RegexPatterns.Password);
        
        RuleFor(command => command.RoleId).ValidRequiredGuid();
        
        RuleFor(command => command.ClinicianId).ValidRequiredGuid();
    }
}