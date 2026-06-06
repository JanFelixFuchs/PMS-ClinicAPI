using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Common.Configuration;

public class CookieSettings
{
    public const string SectionName = "CookieSettings";
    
    [Required]
    public bool Secure { get; init; }
    
    [Required]
    public SameSiteMode SameSiteMode { get; init; }
        
    [Required]
    public bool RestrictPath { get; init; }
}