using Application.Companies.Create;
using Application.Companies.Get;
using Application.Companies.GetById;
using Application.Companies.GetByIsin;
using Application.Companies.Update;
using Carter;
using MediatR;

namespace Web.API.Endpoints;

public class CompanyEndpoints : CarterModule
{
    public CompanyEndpoints() : base("/api/company")
    {
        WithTags("Company");
        RequireAuthorization();
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/", async (ISender sender) =>
        {
            var result = await sender.Send(new GetCompaniesQuery());

            return Results.Ok(result);
        });

        app.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetCompanyByIdQuery(id));

            return Results.Ok(result);
        });

        app.MapGet("/isin/{isin}", async (string isin, ISender sender) =>
        {
            var result = await sender.Send(new GetCompanyByIsinQuery(isin));

            return Results.Ok(result);
        });

        app.MapPost("/", async (CreateCompanyCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);

            return Results.Ok(result);
        });

        app.MapPut("/{id:guid}", async (Guid id, UpdateCompanyCommand command, ISender sender) =>
        {
            var result = await sender.Send(command with { Id = id });

            return Results.Ok(result);
        });
    }
}