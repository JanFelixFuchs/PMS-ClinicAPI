using Application.Common.Behaviours.Validation.Rules;
using Domain.Commons.Utils.Constants;
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