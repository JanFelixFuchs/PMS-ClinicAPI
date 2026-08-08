using Application.Common.Behaviours.ValidationBehaviour.Rules;
using Domain.Common.Utils.Constants;
using FluentValidation;

namespace Application.UseCases.AuthUseCases.Commands.UpdateClinicCodeCommand;

public class UpdateClinicCodeCommandValidator : AbstractValidator<UpdateClinicCodeCommand>
{
    public UpdateClinicCodeCommandValidator()
    {
        RuleFor(command => command.OldCode).ValidRequiredString();

        RuleFor(command => command.NewCode)
            .ValidRequiredString()
            .ValidRequiredRegex(RegexPatterns.Code);
    }
}