namespace Application.Common.Services;

public interface IAuthenticationService
{
    bool CheckPassword(string passwordHash, string rawPassword);
    string ValidateAndHashPassword(string rawPassword);
    string HashToken(string token);
}