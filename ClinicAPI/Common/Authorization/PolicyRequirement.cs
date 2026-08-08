using Domain.Common.Enums;
using Microsoft.AspNetCore.Authorization;

namespace PMS_ClinicAPI.Common.Authorization;

public record PolicyRequirement(
    ClaimType Resource, 
    ClaimValue MinimumAccessLevel)
    : IAuthorizationRequirement;