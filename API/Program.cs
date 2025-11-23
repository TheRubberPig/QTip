using Microsoft.EntityFrameworkCore;
using QTip.Api.Database.DTOs;
using QTip.Api.Interfaces;
using QTip.Api.Services;
using QTip.API.Database;
var builder = WebApplication.CreateBuilder(args);

// Infrastructure
var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING") ?? builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<IPiiService, PiiService>();
builder.Services.AddOpenApi();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDBContext>();
    try
    {
        dbContext.Database.EnsureCreated();
    }
    catch (Exception ex)
    {
        Console.WriteLine("Database migration failed: " + ex.Message);
    }
}

app.MapPost("/pii/submit", async (IPiiService piiService, Request request) =>
{
    var result = await piiService.ProcessPiiAsync(request.text);
    return Results.Ok(result);
});

app.MapGet("/pii/count", async (IPiiService piiService) =>
{
    var count = await piiService.GetPiiCountAsync();
    return Results.Ok(new { emailPiiCount = count });
});

app.MapGet("/status", () => Results.Ok("API is running."));

app.Run();