using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Common.Configuration;

public class CorsSettings
{
    public const string SectionName = "CorsSettings";
    
    [Required]
    public required string[] AllowedOrigins { get; init; }
}