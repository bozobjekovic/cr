using Application.Abstractions.Data;
using Application.Common.Exceptions;
using Application.Companies.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.GetById;

public class GetCompanyByIdQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<GetCompanyByIdQuery, CompanyDto>
{
    private readonly IApplicationDbContext _dbContext = dbContext;

    public async Task<CompanyDto> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
    {
        var company = await _dbContext
            .Companies
            .Where(c => c.Id == request.Id)
            //.FirstOrDefault(c => c.Id == request.Id)
            .Select(c => new CompanyDto(
                c.Id,
                c.Name,
                c.Exchange,
                c.Ticker,
                c.Isin,
                c.Website))
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        if (company is null)
        {
            throw new NotFoundException("Company not found");
        }

        return company;
    }
}