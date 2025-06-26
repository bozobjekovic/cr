using Application.Companies.Dto;
using MediatR;

namespace Application.Companies.Create;

public record CreateCompanyCommand(
    string Name,
    string Exchange,
    string Ticker,
    string Isin,
    string? Website = null) : IRequest<CompanyDto>;