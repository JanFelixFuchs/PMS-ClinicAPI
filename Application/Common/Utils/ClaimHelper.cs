using Domain.Commons.Enums;
using Domain.Commons.Utils.Constants;
using Domain.Entities.IdentityEntities;

namespace Application.Common.Utils;

public static class ClaimHelper
{
    public static ICollection<Claim> CreateHighestPermissionClaims(Role role)
    {
        // Returning claims built based on whitelist dictionary
        return ClaimTypePermissions.AllowedValues
            .Select(claimTypeValuePair => new Claim(role, claimTypeValuePair.Key, claimTypeValuePair.Value.Max()))
            .ToList();
    }

    public static ICollection<Claim> CreateClaimsFromDictionary(Role role, Dictionary<ClaimType, ClaimValue> claims)
    {
        // Returning claims
        return claims
            .Select(claim => new Claim(role, claim.Key, claim.Value))
            .ToList();   
    }
    
    public static ICollection<Claim> FillMissingClaimsWithLowestPermission(Role role, ICollection<Claim> claims)
    {
        // Collecting existing claim types
        var existingClaimTypes = claims.Select(claim => claim.Type).ToHashSet();
        
        // Calculating missing claim types based on whitelist dictionary
        var missingClaimTypes = ClaimTypePermissions.AllowedValues.Keys
            .Except(existingClaimTypes)
            .ToList();
        
        // Adding missing claims
        foreach (var missingClaimType in missingClaimTypes)
            claims.Add(new Claim(role, missingClaimType, ClaimValue.None));
        
        // Returning claims
        return claims;
    }
}