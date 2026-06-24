using Application.Common.Behaviours.Validation.Rules;
using FluentValidation;

namespace Application.UseCases.AuthUseCases.Commands.LoginUserCommand;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(command => command.Code).ValidRequiredString();
        
        RuleFor(command => command.Username).ValidRequiredString();
        
        RuleFor(command => command.Password).ValidRequiredString();
    }
}