using Application.Abstractions.Data;
using Application.Common.Exceptions;
using Application.Companies.GetById;
using Domain.Entities;
using MockQueryable.Moq;
using Moq;

namespace Application.UnitTests.Companies.Queries;

public class GetCompanyByIdQueryHandlerTests
{
    private readonly Mock<IApplicationDbContext> _dbContext = new();

    [Fact]
    public async Task Handle_GetCompanyById_ShouldReturnCompany()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var command = new GetCompanyByIdQuery(companyId);

        var companies = new List<Company>
        {
            new()
            {
                Id = companyId,
                Name = "Company",
                Exchange = "Exchange",
                Ticker = "Ticker",
                Isin = "US0378331005",
                Website = "https://test.com"
            }
        };

        var mockDbSet = companies.AsQueryable().BuildMockDbSet();
        _dbContext.Setup(x => x.Companies).Returns(mockDbSet.Object);

        var handler = new GetCompanyByIdQueryHandler(_dbContext.Object);

        // Act
        var companyDto = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(companyDto);
        Assert.Equal(command.Id, companyDto.Id);
        Assert.Equal("Company", companyDto.Name);
    }

    [Fact]
    public async Task Handle_GetCompanyById_ShouldThrowNotFoundException_WhenCompanyDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();
        var command = new GetCompanyByIdQuery(nonExistentId);

        var mockDbSet = new List<Company>().AsQueryable().BuildMockDbSet();
        _dbContext.Setup(x => x.Companies).Returns(mockDbSet.Object);

        var handler = new GetCompanyByIdQueryHandler(_dbContext.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(
            () => handler.Handle(command, CancellationToken.None));

        Assert.Equal("Company not found", exception.Message);
    }
}