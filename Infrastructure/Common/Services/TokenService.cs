using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Common.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Utils.Authentication;
using Claim = Domain.Entities.IdentityEntities.Claim;
using JwtClaim = System.Security.Claims.Claim;

namespace Infrastructure.Common.Services;

public class TokenService(IConfiguration configuration) : ITokenService
{
    public string CreateAccessToken(Guid clinicId, Guid userId, ICollection<Claim> claims)
    {
        // Accessing configuration
        var jwtSettings = configuration.GetSection("JwtSettings");
        
        // Initializing key and credentials
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.GetValue<string>("Secret")!));
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
            issuer: jwtSettings.GetValue<string>("Issuer"),
            audience: jwtSettings.GetValue<string>("Audience"),
            claims: jwtClaims,
            expires: DateTime.UtcNow.AddMinutes(jwtSettings.GetValue<double>("AccessTokenLifetimeInMinutes")),
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
        // Accessing configuration
        var jwtSettings = configuration.GetSection("JwtSettings");
        
        // Setting token validation parameters
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.GetValue<string>("Issuer"),
            ValidateAudience = true,
            ValidAudience = jwtSettings.GetValue<string>("Audience"),
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.GetValue<string>("Secret")!)),
            ClockSkew = TimeSpan.Zero
        };

        // Validating access token
        var principal = new JwtSecurityTokenHandler().ValidateToken(accessToken, tokenValidationParameters, out _);
        
        // Returning principal
        return principal;
    }
}