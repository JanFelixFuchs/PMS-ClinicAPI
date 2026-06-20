using Claim = Domain.Entities.IdentityEntities.Claim;

namespace Application.Common.Services;

public interface ITokenService
{
    string CreateAccessToken(Guid clinicId, Guid userId, ICollection<Claim> claims);
    string CreateRefreshToken();
}