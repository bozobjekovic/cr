using Carter;
using Identity.API.Data;
using Microsoft.AspNetCore.Identity;

namespace Identity.API.Endpoints;

public class RegistrationEndpoints : CarterModule
{
    public RegistrationEndpoints() : base("/api/auth")
    {
        WithTags("Auth");
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/register", async (RegisterRequest request, UserManager<AppUser> userManager) =>
        {
            var user = new AppUser(request.Email);
            var result = await userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                return Results.Ok(new
                {
                    Message = $"User {request.Email} created successfully",
                    UserId = user.Id
                });
            }

            return Results.BadRequest(new
            {
                Errors = result.Errors.Select(e => e.Description)
            });
        });
    }
}

public sealed record RegisterRequest(string Email, string Password);