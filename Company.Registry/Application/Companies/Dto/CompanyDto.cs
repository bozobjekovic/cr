namespace Application.Companies.Dto;

public record CompanyDto(
    Guid Id,
    string Name,
    string Exchange,
    string Ticker,
    string Isin,
    string? Website);