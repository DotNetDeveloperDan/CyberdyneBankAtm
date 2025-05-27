using CyberdyneBankAtm.Api.Utilities;
using CyberdyneBankAtm.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace CyberdyneBankAtm.Api.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        using var dbContext =
            scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        dbContext.Database.Migrate();
        //Seed the database if it's empty
        DbInitializer.Seed(dbContext);
    }
}