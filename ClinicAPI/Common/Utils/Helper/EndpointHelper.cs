using Microsoft.AspNetCore.Mvc.Controllers;

namespace PMS_ClinicAPI.Common.Utils.Helper;

public static class EndpointHelper
{
    public static string GetEndpointName(HttpContext context)
    {
        // Getting endpoint
        var endpoint = context.GetEndpoint();
        
        // Extracting action descriptor
        var actionDescriptor = endpoint?.Metadata.GetMetadata<ControllerActionDescriptor>();
        
        // Returning endpoint name
        return actionDescriptor != null 
            ? actionDescriptor.ActionName
            : endpoint?.DisplayName ?? "Unknown";
    }
}