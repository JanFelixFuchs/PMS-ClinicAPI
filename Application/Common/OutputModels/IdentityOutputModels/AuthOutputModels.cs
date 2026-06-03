using Application.Common.OutputModels.SharedOutputModels;
using Domain.Entities.ClinicianEntities;
using Domain.Entities.IdentityEntities;

namespace Application.Common.OutputModels.IdentityOutputModels;

public class RegisterClinicOutputModel(
    Clinic clinic,
    User user,
    Role role,
    Clinician? clinician,
    string accessToken,
    string refreshToken)
{
    public ClinicOutputModel Clinic { get; init; } = new(clinic);
    public CurrentUserOutputModel User { get; init; } = new(user, role, clinician);
    public TokensOutputModel Tokens { get; init; } = new(accessToken, refreshToken);
}

public class LoginUserOutputModel(
    Clinic clinic,
    User user,
    Role role,
    Clinician? clinician,
    string accessToken, 
    string refreshToken)
{
    public ClinicOutputModel Clinic { get; init; } = new(clinic);
    public CurrentUserOutputModel User { get; init; } = new(user, role, clinician);
    public TokensOutputModel Tokens { get; init; } = new(accessToken, refreshToken);
}

public class RefreshTokensOutputModel(string accessToken, string refreshToken)
    : TokensOutputModel(accessToken, refreshToken);

public class UpdatePasswordOutputModel(string accessToken, string refreshToken)
    : TokensOutputModel(accessToken, refreshToken);

public class UpdateUsernameOutputModel(User user, Role role, Clinician? clinician)
    : CurrentUserOutputModel(user, role, clinician);