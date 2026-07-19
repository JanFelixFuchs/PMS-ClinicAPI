using System.Reflection;
using Application.Common.Behaviours.LoggingBehaviour;
using Application.Common.Behaviours.RequestContextBehaviour;
using Application.Common.Behaviours.Validation;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static void AddApplicationLayerDependencies(this IServiceCollection serviceCollection)
    {
        // Adding MediatR
        serviceCollection.AddMediatR(serviceConfiguration =>
            serviceConfiguration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        // Adding pipeline behaviors
        serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
        serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestContextBehaviour<,>));

        // Adding validators
        serviceCollection.AddValidatorsFromAssembly(Assembly.Load("Application"));
    }
}