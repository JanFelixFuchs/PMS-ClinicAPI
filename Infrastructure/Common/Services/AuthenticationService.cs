using System.Security.Cryptography;
using System.Text;
using Application.Common.Services;
using Domain.Commons.Utils.Validation;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Common.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly PasswordHasher<object> _passwordHasher = new();
    
    public bool CheckPassword(string passwordHash, string rawPassword)
    {
        // Checking password
        var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(new object(), passwordHash, rawPassword);
        
        // Returning password verification result
        return passwordVerificationResult is PasswordVerificationResult.Success 
            or PasswordVerificationResult.SuccessRehashNeeded;
    }
    
    public string ValidateAndHashPassword(string rawPassword)
    {
        // Validating raw password
        PasswordValidationPolicy.Validate(rawPassword);
        
        // Returning hashed password
        return _passwordHasher.HashPassword(new object(), rawPassword);
    }
    
    public string HashToken(string token)
    {
        // Returning hashed token
        return Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(token)));
    }
}