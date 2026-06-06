using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Common.Configuration;

public class JwtSettings
{
    public const string SectionName = "JwtSettings";
    
    [Required]
    public string Issuer { get; init; } = string.Empty;
    
    [Required]
    public string Audience { get; init; } = string.Empty;
    
    [Required]
    public string Secret { get; init; } = string.Empty;
    
    [Required, Range(1, 1440)]
    public int AccessTokenLifetimeInMinutes { get; init; }
    
    [Required, Range(1, 365)]
    public int RefreshTokenLifetimeInDays { get; init; }
}