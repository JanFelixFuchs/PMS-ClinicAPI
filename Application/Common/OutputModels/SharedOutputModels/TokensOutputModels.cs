namespace Application.Common.OutputModels.SharedOutputModels;

public class TokensOutputModel(string accessToken, string refreshToken)
{
    public string AccessToken { get; init; } = accessToken;
    public string RefreshToken { get; init; } = refreshToken;
}