using Application.Abstractions.Data;
using Application.Common.Exceptions;
using Application.Companies.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.GetByIsin;

public class GetCompanyByIsinQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<GetCompanyByIsinQuery, CompanyDto>
{
    private readonly IApplicationDbContext _dbContext = dbContext;

    public async Task<CompanyDto> Handle(GetCompanyByIsinQuery request, CancellationToken cancellationToken)
    {
        var company = await _dbContext
            .Companies
            .Where(c => c.Isin == request.Isin)
            .Select(c => new CompanyDto(
                c.Id,
                c.Name,
                c.Exchange,
                c.Ticker,
                c.Isin,
                c.Website))
            .AsNoTracking()
            .SingleOrDefaultAsync(cancellationToken);

        if (company is null)
        {
            throw new NotFoundException("Company not found");
        }

        return company;
    }
}