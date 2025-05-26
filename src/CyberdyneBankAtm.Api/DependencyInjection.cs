using CyberdyneBankAtm.Api.Infrastructure;

namespace CyberdyneBankAtm.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddOpenApiDocument(config =>
        {
            config.Title = "CyberdyneBankAtm.Api";
            config.Version = "v1";
            // Optional: Add further configuration
        });

        // REMARK: If you want to use Controllers, you'll need this.
        services.AddControllers();

        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }
}