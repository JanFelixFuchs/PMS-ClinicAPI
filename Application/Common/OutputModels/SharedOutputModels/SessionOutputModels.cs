using Application.Common.OutputModels.IdentityOutputModels;
using Domain.Entities.ClinicianEntities;
using Domain.Entities.IdentityEntities;

namespace Application.Common.OutputModels.SharedOutputModels;

public class SessionOutputModel(
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