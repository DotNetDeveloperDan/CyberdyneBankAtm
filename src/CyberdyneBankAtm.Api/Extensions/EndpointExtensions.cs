using System.Reflection;
using CyberdyneBankAtm.Api.Endpoints;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CyberdyneBankAtm.Api.Extensions;

public static class EndpointExtensions
{
    public static IServiceCollection AddEndpoints(this IServiceCollection services, Assembly assembly)
    {
        var serviceDescriptors = assembly
            .DefinedTypes
            .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                           type.IsAssignableTo(typeof(IEndpoint)))
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
            .ToArray();

        services.TryAddEnumerable(serviceDescriptors);

        return services;
    }

    /// <summary>
    ///     Maps all registered <see cref="IEndpoint" /> implementations to the application's endpoint routing.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication" /> instance to map endpoints on.</param>
    /// <param name="routeGroupBuilder">
    ///     Optional <see cref="RouteGroupBuilder" /> to group endpoints under a specific route group.
    ///     If null, endpoints are mapped directly to the application's root route builder.
    /// </param>
    /// <returns>The <see cref="IApplicationBuilder" /> instance for chaining.</returns>
    public static IApplicationBuilder MapEndpoints(
        this WebApplication app,
        RouteGroupBuilder? routeGroupBuilder = null)
    {
        var endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();

        IEndpointRouteBuilder builder = routeGroupBuilder is null ? app : routeGroupBuilder;

        foreach (var endpoint in endpoints) endpoint.MapEndpoint(builder);

        return app;
    }
}