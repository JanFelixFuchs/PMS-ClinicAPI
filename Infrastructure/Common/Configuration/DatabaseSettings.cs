using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Common.Configuration;

public class DatabaseSettings
{
    public const string SectionName = "DatabaseSettings";
    
    [Required]
    public string Server { get; init; } = string.Empty;
    
    [Required]
    public string Database { get; init; } = string.Empty;
    
    [Required]
    public string User { get; init; } = string.Empty;
    
    [Required]
    public string Password { get; init; } = string.Empty;
    
    [Required]
    public string Port { get; init; } = string.Empty;
    
    [Required]
    public string Version { get; init; } = string.Empty;
}