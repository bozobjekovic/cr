using Application.Abstractions.Data;
using Application.Companies.Dto;
using Domain.Entities;
using MediatR;

namespace Application.Companies.Create;

public class CreateCompanyCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<CreateCompanyCommand, CompanyDto>
{
    private readonly IApplicationDbContext _dbContext = dbContext;

    public async Task<CompanyDto> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        var company = new Company
        {
            Name = request.Name,
            Exchange = request.Exchange,
            Ticker = request.Ticker,
            Isin = request.Isin,
            Website = request.Website,
        };

        _dbContext.Companies.Add(company);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new CompanyDto(
            company.Id,
            company.Name,
            company.Exchange,
            company.Ticker,
            company.Isin,
            company.Website);
    }
}