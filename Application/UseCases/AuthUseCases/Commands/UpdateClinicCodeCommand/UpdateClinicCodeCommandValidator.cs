using Application.Common.Behaviours.Validation.Rules;
using Domain.Commons.Utils.Constants;
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