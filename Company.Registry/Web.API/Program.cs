using Application;
using Application.Common.Configuration;
using Carter;
using Infrastructure;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Web.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((_, loggerConfig) =>
    loggerConfig.WriteTo.Console());

var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtOptions>()
                  ?? throw new ApplicationException("Missing JWT configuration");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = jwtSettings.Authority;
        options.Audience = jwtSettings.Audience;
        options.RequireHttpsMetadata = false;
    });

builder.Services.AddAuthorization();

builder.Services.AddCarter(configurator:
    config => { config.WithValidatorLifetime(ServiceLifetime.Scoped); });

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await using (var serviceScope = app.Services.CreateAsyncScope())
    await using (var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>())
    {
        await dbContext.Database.MigrateAsync();
    }

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapCarter();

app.UseHttpsRedirection();

app.UseSerilogRequestLogging();

app.Run();

public partial class Program;