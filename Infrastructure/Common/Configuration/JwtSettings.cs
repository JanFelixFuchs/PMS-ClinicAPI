using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Common.Configuration;

public class JwtSettings
{
    public const string SectionName = "JwtSettings";
    
    [Required]
    public required string Issuer { get; init; }
    
    [Required]
    public required string Audience { get; init; }
    
    [Required]
    public required string Secret { get; init; }
    
    [Required, Range(1, 1440)]
    public required int AccessTokenLifetimeInMinutes { get; init; }
    
    [Required, Range(1, 365)]
    public required int RefreshTokenLifetimeInDays { get; init; }
}