using Application.Common.OutputModels.SharedOutputModels;
using Domain.Entities.ClinicianEntities;
using Domain.Entities.IdentityEntities;

namespace Application.Common.OutputModels.IdentityOutputModels;

public class RegisterClinicOutputModels(Clinic clinic, User user, Role role, Clinician? clinician, string accessToken)
    : SessionOutputModel(clinic, user, role, clinician, accessToken);

public class LoginUserOutputModels(Clinic clinic, User user, Role role, Clinician? clinician, string accessToken)
    : SessionOutputModel(clinic, user, role, clinician, accessToken);

public class RefreshTokensOutputModels(Clinic clinic, User user, Role role, Clinician? clinician, string accessToken)
    : SessionOutputModel(clinic, user, role, clinician, accessToken);

public class UpdatePasswordOutputModel(string accessToken)
{
    public string AccessToken { get; init; } = accessToken;
}

public class UpdateUsernameOutputModel(User user, Role role, Clinician? clinician)
    : CurrentUserOutputModel(user, role, clinician);