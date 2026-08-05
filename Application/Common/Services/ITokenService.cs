using Claim = Domain.Entities.IdentityEntities.Claim;

namespace Application.Common.Services;

public interface ITokenService
{
    // Methods
    string CreateAccessToken(Guid clinicId, Guid userId, ICollection<Claim> claims);
    string CreateRefreshToken();
}