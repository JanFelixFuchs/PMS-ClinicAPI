using Microsoft.AspNetCore.Authorization;
using PMS_ClinicAPI.Common.Authorization;

namespace PMS_ClinicAPI;

public static class DependencyInjection
{
    public static void AddApiLayerDependencies(this IServiceCollection serviceCollection)
    {
        // Adding custom authorization handler
        serviceCollection.AddSingleton<IAuthorizationHandler, PolicyHandler>();

        // Adding http context accessor
        serviceCollection.AddHttpContextAccessor();
    }
}