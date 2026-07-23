using Domain.Commons.Enums;
using Microsoft.AspNetCore.Authorization;
using PMS_ClinicAPI.Common.Logging;
using Utils.Authentication;

namespace PMS_ClinicAPI.Common.Authorization;

public class PolicyHandler(ILogger<PolicyHandler> logger) 
    : AuthorizationHandler<PolicyRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext authorizationHandlerContext, 
        PolicyRequirement policyRequirement)
    {
        // Extracting and checking claim
        var claim = authorizationHandlerContext.User.FindFirst($"{ClaimNames.PermissionPrefix}{policyRequirement.Resource}");
        if (claim == null)
        {
            logger.LogWarning(LogMessages.MissingPolicyClaim, policyRequirement.Resource.ToString());
            authorizationHandlerContext.Fail();
            return Task.CompletedTask;
        }
        
        // Checking for valid access level
        if (!Enum.TryParse<ClaimValue>(claim.Value, true, out var accessLevel))
        {
            logger.LogWarning(LogMessages.InvalidPolicyClaimValue, policyRequirement.Resource.ToString(), claim.Value);  
            authorizationHandlerContext.Fail();
            return Task.CompletedTask;
        }
        
        // Verifying access level
        if (accessLevel >= policyRequirement.MinimumAccessLevel)
        {
            logger.LogInformation(LogMessages.AuthorizationSucceeded, policyRequirement.Resource.ToString(), policyRequirement.MinimumAccessLevel.ToString(),  accessLevel.ToString());
            authorizationHandlerContext.Succeed(policyRequirement);
        }
        else
        {
            logger.LogWarning(LogMessages.InsufficientAccessLevel, policyRequirement.Resource.ToString(), policyRequirement.MinimumAccessLevel.ToString(), accessLevel.ToString());
            authorizationHandlerContext.Fail();
        }
        
        // Completing task
        return Task.CompletedTask;
    }
}