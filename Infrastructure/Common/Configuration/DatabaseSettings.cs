using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Common.Configuration;

public class DatabaseSettings
{
    public const string SectionName = "DatabaseSettings";
    
    [Required]
    public required string Server { get; init; }
    
    [Required]
    public required string Database { get; init; }
    
    [Required]
    public required string User { get; init; }
    
    [Required]
    public required string Password { get; init; }
    
    [Required]
    public required string Port { get; init; }
    
    [Required]
    public required string Version { get; init; }
}