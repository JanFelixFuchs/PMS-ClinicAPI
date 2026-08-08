using Application.Common.Behaviours.ValidationBehaviour.Rules;
using Domain.Common.Utils.Constants;
using FluentValidation;

namespace Application.UseCases.AuthUseCases.Commands.UpdateUsernameCommand;

public class UpdateUsernameCommandValidator : AbstractValidator<UpdateUsernameCommand>
{
    public UpdateUsernameCommandValidator()
    {
        RuleFor(command => command.OldUsername).ValidRequiredString();
        
        RuleFor(command => command.NewUsername)
            .ValidRequiredString()
            .ValidRequiredRegex(RegexPatterns.Username);
    }
}