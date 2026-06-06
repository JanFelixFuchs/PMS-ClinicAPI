using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Common.Services;
using Infrastructure.Common.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Utils.Authentication;
using Claim = Domain.Entities.IdentityEntities.Claim;
using JwtClaim = System.Security.Claims.Claim;

namespace Infrastructure.Common.Services;

public class TokenService(IOptions<JwtSettings> jwtSettings) : ITokenService
{
    public string CreateAccessToken(Guid clinicId, Guid userId, ICollection<Claim> claims)
    {
        // Initializing key and credentials
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Value.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
    
        // Initializing claim list
        var jwtClaims = new List<JwtClaim>
        {
            new (ClaimNames.ClinicId, clinicId.ToString()),
            new (ClaimNames.UserId, userId.ToString()),
        };
        
        // Adding role specific claims 
        jwtClaims.AddRange(claims.Select(claim => new JwtClaim($"{ClaimNames.PermissionPrefix}{claim.Type}", claim.Value.ToString())));
    
        // Creating access token
        var accessToken = new JwtSecurityToken(
            issuer: jwtSettings.Value.Issuer,
            audience: jwtSettings.Value.Audience,
            claims: jwtClaims,
            expires: DateTime.UtcNow.AddMinutes(jwtSettings.Value.AccessTokenLifetimeInMinutes),
            signingCredentials: credentials);
        
        // Returning access token
        return new JwtSecurityTokenHandler().WriteToken(accessToken);
    }
    
    public string CreateRefreshToken()
    {
        // Returning refresh token
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }
    
    public ClaimsPrincipal GetPrincipalFromExpiredAccessToken(string accessToken)
    {
        // Setting token validation parameters
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Value.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtSettings.Value.Audience,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Value.Secret)),
            ClockSkew = TimeSpan.Zero
        };

        // Validating access token
        var principal = new JwtSecurityTokenHandler().ValidateToken(accessToken, tokenValidationParameters, out _);
        
        // Returning principal
        return principal;
    }
}