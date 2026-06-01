using Domain.Commons.Enums;
using Domain.Entities.IdentityEntities;

namespace Application.Common.OutputModels.SharedOutputModels;

public class ClaimOutputModel(Claim claim)
{
    public Guid Id { get; init; } = claim.Id;
    public ClaimType Type { get; init; } = claim.Type;
    public ClaimValue Value { get; init; } = claim.Value;
}