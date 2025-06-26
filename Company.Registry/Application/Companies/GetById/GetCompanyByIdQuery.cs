using Application.Companies.Dto;
using MediatR;

namespace Application.Companies.GetById;

public record GetCompanyByIdQuery(
    Guid Id) : IRequest<CompanyDto>;