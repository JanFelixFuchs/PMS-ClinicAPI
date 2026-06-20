using Application.Common.OutputModels.SharedOutputModels;
using Domain.Entities.ClinicianEntities;
using Domain.Entities.IdentityEntities;

namespace Application.Common.OutputModels.IdentityOutputModels;

public class RegisterClinicOutputModel(
    Clinic clinic,
    User user,
    Role role,
    Clinician? clinician,
    string accessToken)
{
    public ClinicOutputModel Clinic { get; init; } = new(clinic);
    public CurrentUserOutputModel User { get; init; } = new(user, role, clinician);
    public string AccessToken { get; init; } = accessToken;
}

public class LoginUserOutputModel(
    Clinic clinic,
    User user,
    Role role,
    Clinician? clinician,
    string accessToken)
{
    public ClinicOutputModel Clinic { get; init; } = new(clinic);
    public CurrentUserOutputModel User { get; init; } = new(user, role, clinician);
    public string AccessToken { get; init; } = accessToken;
}

public class RefreshTokensOutputModel(
    Clinic clinic,
    User user,
    Role role,
    Clinician? clinician,
    string accessToken)
{
    public ClinicOutputModel Clinic { get; init; } = new(clinic);
    public CurrentUserOutputModel User { get; init; } = new(user, role, clinician);
    public string AccessToken { get; init; } = accessToken;
}

public class UpdatePasswordOutputModel(string accessToken)
{
    public string AccessToken { get; init; } = accessToken;
}

public class UpdateUsernameOutputModel(User user, Role role, Clinician? clinician)
    : CurrentUserOutputModel(user, role, clinician);