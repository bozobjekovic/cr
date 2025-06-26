using Application.Companies.Dto;
using MediatR;

namespace Application.Companies.GetByIsin;

public record GetCompanyByIsinQuery(
    string Isin) : IRequest<CompanyDto>;