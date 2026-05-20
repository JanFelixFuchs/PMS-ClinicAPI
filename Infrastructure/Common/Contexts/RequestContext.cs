using System.Security.Claims;
using Application.Common.Contexts;
using Infrastructure.Common.Exceptions.Contexts;
using Microsoft.AspNetCore.Http;
using Utils.Authentication;

namespace Infrastructure.Common.Contexts;

public class RequestContext(IHttpContextAccessor httpContextAccessor) : IRequestContext
{
    private readonly ClaimsPrincipal? _user = httpContextAccessor.HttpContext?.User;
    
    public Guid ClinicId => GetGuidClaim(ClaimNames.ClinicId);
    public Guid UserId => GetGuidClaim(ClaimNames.UserId);
    
    private Guid GetGuidClaim(string claimName)
    {
        var claim = _user?.FindFirst(claimName);
        return claim != null && Guid.TryParse(claim.Value, out var guid)
            ? guid
            : throw new InvalidRequestContextException(claimName);
    }
}