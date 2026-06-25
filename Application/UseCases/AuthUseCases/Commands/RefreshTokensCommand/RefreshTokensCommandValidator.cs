using Application.Common.Behaviours.Validation.Rules;
using FluentValidation;

namespace Application.UseCases.AuthUseCases.Commands.RefreshTokensCommand;

public class RefreshTokensCommandValidator : AbstractValidator<RefreshTokensCommand>
{
    public RefreshTokensCommandValidator()
    {
        RuleFor(command => command.RefreshToken).ValidRequiredString();
    }
}