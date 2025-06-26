using Application.Abstractions.Data;
using Application.Companies.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Update;

public class UpdateCompanyCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<UpdateCompanyCommand, CompanyDto>
{
    private readonly IApplicationDbContext _dbContext = dbContext;

    public async Task<CompanyDto> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
    {
        var company = await _dbContext
            .Companies
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (company is null)
        {
            throw new Exception("Company not found");
        }

        company.Name = request.Name;
        company.Exchange = request.Exchange;
        company.Ticker = request.Ticker;
        company.Isin = request.Isin;
        company.Website = request.Website;

        _dbContext.Companies.Update(company);

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