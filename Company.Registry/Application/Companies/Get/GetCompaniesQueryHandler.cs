using Application.Abstractions.Data;
using Application.Companies.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Get;

public class GetCompaniesQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<GetCompaniesQuery, IEnumerable<CompanyDto>>
{
    private readonly IApplicationDbContext _dbContext = dbContext;

    public async Task<IEnumerable<CompanyDto>> Handle(GetCompaniesQuery request, CancellationToken cancellationToken)
    {
        var companies = await _dbContext
            .Companies
            .OrderBy(c => c.Name)
            .Select(c => new CompanyDto(
                c.Id,
                c.Name,
                c.Exchange,
                c.Ticker,
                c.Isin,
                c.Website))
            .ToListAsync(cancellationToken);

        return companies;
    }
}