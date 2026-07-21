using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Common.Configuration;

public class CookieSettings
{
    public const string SectionName = "CookieSettings";
    
    [Required]
    public required bool Secure { get; init; }
    
    [Required]
    public required SameSiteMode SameSiteMode { get; init; }
        
    [Required]
    public required bool RestrictPath { get; init; }
}