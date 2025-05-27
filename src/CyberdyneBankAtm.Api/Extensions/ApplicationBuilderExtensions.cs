using Microsoft.AspNetCore.Builder;
using NSwag;
namespace CyberdyneBankAtm.Api.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseSwaggerWithUi(this WebApplication app)
    {
        // Serves the OpenAPI JSON at /swagger/v1/swagger.json
        app.UseOpenApi();
        // Serves the Swagger UI at /swagger
        app.UseSwaggerUi();
        return app;


    }
}