using Application.Common.Behaviours.Validation.Rules;
using Domain.Commons.Utils.Constants;
using FluentValidation;

namespace Application.UseCases.AuthUseCases.Commands.UpdatePasswordCommand;

public class UpdatePasswordCommandValidator : AbstractValidator<UpdatePasswordCommand>
{
    public UpdatePasswordCommandValidator()
    {
        RuleFor(command => command.OldPassword).ValidRequiredString();
        
        RuleFor(command => command.NewPassword)
            .ValidRequiredString()
            .ValidRequiredRegex(RegexPatterns.Password);
    }
}