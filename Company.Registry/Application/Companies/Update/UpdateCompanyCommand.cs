using Application.Companies.Dto;
using MediatR;

namespace Application.Companies.Update;

public record UpdateCompanyCommand(
    Guid Id,
    string Name,
    string Exchange,
    string Ticker,
    string Isin,
    string? Website) : IRequest<CompanyDto>;