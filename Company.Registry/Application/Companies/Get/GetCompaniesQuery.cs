using Application.Companies.Dto;
using MediatR;

namespace Application.Companies.Get;

public record GetCompaniesQuery : IRequest<IEnumerable<CompanyDto>>;