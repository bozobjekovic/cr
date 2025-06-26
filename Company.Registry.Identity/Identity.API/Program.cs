using Carter;
using Identity.API.Data;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((_, loggerConfig) =>
    loggerConfig.WriteTo.Console());

builder.Services.AddCarter();

builder.Services.AddDbContext<ApplicationIdentityDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Database")));

builder.Services.AddIdentity<AppUser, IdentityRole<Guid>>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 6;

        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddIdentityServer(options =>
    {
        options.Events.RaiseErrorEvents = true;
        options.Events.RaiseInformationEvents = true;
        options.Events.RaiseFailureEvents = true;
        options.Events.RaiseSuccessEvents = true;
    })
    .AddInMemoryApiScopes(GetApiScopes())
    .AddInMemoryApiResources(GetApiResources())
    .AddInMemoryClients(GetClients())
    .AddInMemoryIdentityResources(GetIdentityResources())
    .AddAspNetIdentity<AppUser>()
    .AddDeveloperSigningCredential();

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

await using (var serviceScope = app.Services.CreateAsyncScope())
await using (var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationIdentityDbContext>())
{
    await dbContext.Database.MigrateAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
    app.UseDeveloperExceptionPage();
}

app.UseRouting();
app.UseCors();

app.UseIdentityServer();

app.MapCarter();

app.UseSerilogRequestLogging();

app.Run();

static IEnumerable<ApiScope> GetApiScopes()
{
    return
    [
        new ApiScope("company-api", "Company Registry API")
    ];
}

static IEnumerable<ApiResource> GetApiResources()
{
    return
    [
        new ApiResource("company-api", "Company Registry API")
        {
            Scopes = { "company-api" }
        }
    ];
}

static IEnumerable<Client> GetClients()
{
    return
    [
        new Client
        {
            ClientId = "company-client",
            ClientName = "Company API Client",
            AllowedGrantTypes = GrantTypes.ClientCredentials,
            ClientSecrets = { new Secret("company-secret".Sha256()) },
            AllowedScopes = { "company-api" }
        },
        new Client
        {
            ClientId = "company-user-client",
            ClientName = "Company User Client",
            AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
            ClientSecrets = { new Secret("user-secret".Sha256()) },
            AllowedScopes = { "company-api", "openid", "profile" }
        }
    ];
}

static IEnumerable<IdentityResource> GetIdentityResources()
{
    return
    [
        new IdentityResources.OpenId(),
        new IdentityResources.Profile()
    ];
}