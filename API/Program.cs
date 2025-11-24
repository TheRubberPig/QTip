using Microsoft.EntityFrameworkCore;
using QTip.Api.Models;
using QTip.Api.Interfaces;
using QTip.Api.Services;
using QTip.API.Database;
var builder = WebApplication.CreateBuilder(args);

// Infrastructure
var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING") ?? builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<IPiiService, PiiService>();
builder.Services.AddSingleton<IPatternMatchingService, PatternMatchingService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("QTipCorsPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

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

app.UseCors("QTipCorsPolicy");

app.MapPost("/pii/submit", async (IPiiService piiService, SubmitPii request) =>
{
    var result = await piiService.ProcessPiiAsync(request.text);
    return Results.Ok(result);
});

app.MapGet("/pii/{type}/count", async (IPiiService piiService, ClassificationTypes type) =>
{
    var stats = await piiService.GetPiiCountAsync(type);
    return Results.Ok(stats);
});

app.MapGet("/pii/count", async (IPiiService piiService) =>
{
    var stats = await piiService.GetPiiCountAsync();
    return Results.Ok(stats);
});

app.MapGet("/status", () => Results.Ok("API is running."));

app.Run();