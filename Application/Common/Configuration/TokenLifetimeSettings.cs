using System.ComponentModel.DataAnnotations;

namespace Application.Common.Configuration;

public class TokenLifetimeSettings
{
    public const string SectionName = "JwtSettings";
    
    [Required, Range(1, 1440)]
    public int AccessTokenLifetimeInMinutes { get; init; }
    
    [Required, Range(1, 365)]
    public int RefreshTokenLifetimeInDays { get; init; }
}