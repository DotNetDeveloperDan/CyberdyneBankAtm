using CyberdyneBankAtm.Application.Abstractions.Authentication;
using CyberdyneBankAtm.Application.Abstractions.Data;
using CyberdyneBankAtm.Infrastructure.Authentication;
using CyberdyneBankAtm.Infrastructure.Database;
using CyberdyneBankAtm.Infrastructure.Time;
using CyberdyneBankAtm.SharedKernel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CyberdyneBankAtm.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services
            .AddServices()
            .AddDatabase(configuration)
            .AddAuthenticationInternal(configuration);
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database");

        services.AddDbContext<ApplicationDbContext>(options => options
            .UseSqlite(connectionString)); // Changed to SQLite

        services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());

        return services;
    }


    //Method Subset. We Are not using Authentication
    private static IServiceCollection AddAuthenticationInternal(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<IUserContext, UserContext>();
        return services;

    }
}