using Application.Common.Behaviours.Validation.Rules;
using Domain.Commons.Enums;
using Domain.Commons.Utils.Constants;
using FluentValidation;

namespace Application.UseCases.RoleUseCases.Commands.UpdateRoleCommand;

public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
{
    public UpdateRoleCommandValidator()
    {
        RuleFor(command => command.Id).ValidRequiredGuid();

        RuleFor(command => command.Name)
            .ValidRequiredString()
            .ValidRequiredMaximumStringLength(Lengths.RoleName);
        
        RuleFor(command => command.Claims)
            .NotNull()
            .Must(claims => claims.Keys.ToHashSet().SetEquals(Enum.GetValues<ClaimType>()))
            .WithMessage("{PropertyName} must contain exactly all claims types");
        RuleForEach(command => command.Claims)
            .Must(claim => ClaimTypePermissions.IsAllowed(claim.Key, claim.Value))
            .WithMessage((_, claim) => $"Claim type {claim.Key} must contain only allowed claim values");
    }
}