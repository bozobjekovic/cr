using Application.Abstractions.Data;
using Application.Common.Exceptions;
using Application.Companies.GetByIsin;
using Domain.Entities;
using MockQueryable.Moq;
using Moq;

namespace Application.UnitTests.Companies.Queries;

public class GetCompanyByIsinQueryHandlerTests
{
    private readonly Mock<IApplicationDbContext> _dbContext = new();

    [Fact]
    public async Task Handle_GetCompanyByIsin_ShouldReturnCompany()
    {
        // Arrange
        const string testIsin = "US0378331005";
        var companyId = Guid.NewGuid();
        var command = new GetCompanyByIsinQuery(testIsin);

        var companies = new List<Company>
        {
            new()
            {
                Id = companyId,
                Name = "Company",
                Exchange = "Exchange",
                Ticker = "Ticker",
                Isin = testIsin,
                Website = "https://test.com"
            }
        };

        var mockDbSet = companies.AsQueryable().BuildMockDbSet();
        _dbContext.Setup(x => x.Companies).Returns(mockDbSet.Object);

        var handler = new GetCompanyByIsinQueryHandler(_dbContext.Object);

        // Act
        var companyDto = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(companyDto);
        Assert.Equal(companyId, companyDto.Id);
        Assert.Equal(testIsin, companyDto.Isin);
        Assert.Equal("Company", companyDto.Name);
    }

    [Fact]
    public async Task Handle_GetCompanyByIsin_ShouldThrowNotFoundException_WhenCompanyDoesNotExist()
    {
        // Arrange
        const string nonExistentIsin = "US9999999999";
        var command = new GetCompanyByIsinQuery(nonExistentIsin);

        var mockDbSet = new List<Company>().AsQueryable().BuildMockDbSet();
        _dbContext.Setup(x => x.Companies).Returns(mockDbSet.Object);

        var handler = new GetCompanyByIsinQueryHandler(_dbContext.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(
            () => handler.Handle(command, CancellationToken.None));

        Assert.Equal("Company not found", exception.Message);
    }
}