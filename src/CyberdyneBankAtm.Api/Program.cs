using CyberdyneBankAtm.Api.Extensions;
using CyberdyneBankAtm.Application;
using CyberdyneBankAtm.Infrastructure;
using Serilog;
using System.Reflection;
using CyberdyneBankAtm.Api;

var builder = WebApplication.CreateBuilder(args);

// ────────────────────────────────────────────────────────────────
// Serilog
builder.Host.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));

// ────────────────────────────────────────────────────────────────
// Your services
builder.Services
    .AddApplication()
    .AddPresentation()
    .AddInfrastructure(builder.Configuration);

// register Minimal-API explorer for NSwag
builder.Services.AddEndpointsApiExplorer();

// register your minimal endpoints helper
builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());


var app = builder.Build();

// ────────────────────────────────────────────────────────────────


// if you have controller-based APIs, uncomment:
 app.MapControllers();

// your other minimal endpoints…

app.MapEndpoints();

// ────────────────────────────────────────────────────────────────
// Dev only: serve the docs
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();  // /swagger/v1/swagger.json
    app.UseSwaggerUi(settings =>
    {
        // sort each tag’s operations alphabetically by path
        settings.OperationsSorter = "alpha";
        // sort the tag groups alphabetically
        settings.TagsSorter = "alpha";
    });
    // /swagger
    app.ApplyMigrations();

    app.UseDeveloperExceptionPage();
}

// ────────────────────────────────────────────────────────────────
// standard pipeline
app.UseHttpsRedirection();
app.UseRequestContextLogging();
app.UseSerilogRequestLogging();
app.UseExceptionHandler();
app.UseAuthorization();

await app.RunAsync();