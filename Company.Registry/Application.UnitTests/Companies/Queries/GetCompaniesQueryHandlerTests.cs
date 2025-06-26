using Application.Abstractions.Data;
using Application.Companies.Get;
using Domain.Entities;
using MockQueryable.Moq;
using Moq;

namespace Application.UnitTests.Companies.Queries;

public class GetCompaniesQueryHandlerTests
{
    private readonly Mock<IApplicationDbContext> _dbContext = new();

    [Fact]
    public async Task Handle_ShouldReturnAllCompanies_OrderedByName()
    {
        // Arrange
        var query = new GetCompaniesQuery();

        var companies = new List<Company>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "First Company",
                Exchange = "Exchange",
                Ticker = "Ticker",
                Isin = "US0378331005"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Second Company",
                Exchange = "Exchange",
                Ticker = "Ticker",
                Isin = "US0378331006"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Third Company",
                Exchange = "Exchange",
                Ticker = "Ticker",
                Isin = "US0378331007"
            }
        };

        var mockDbSet = companies.AsQueryable().BuildMockDbSet();
        _dbContext.Setup(x => x.Companies).Returns(mockDbSet.Object);

        var handler = new GetCompaniesQueryHandler(_dbContext.Object);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        var companiesList = result.ToList();
        Assert.Equal(3, companiesList.Count);
        
        Assert.Equal("First Company", companiesList[0].Name);
        Assert.Equal("Second Company", companiesList[1].Name);
        Assert.Equal("Third Company", companiesList[2].Name);
        
        var firstCompany = companiesList.First(c => c.Name == "First Company");
        Assert.Equal("Exchange", firstCompany.Exchange);
        Assert.Equal("Ticker", firstCompany.Ticker);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoCompaniesExist()
    {
        // Arrange
        var query = new GetCompaniesQuery();

        var mockDbSet = new List<Company>().AsQueryable().BuildMockDbSet();
        _dbContext.Setup(x => x.Companies).Returns(mockDbSet.Object);

        var handler = new GetCompaniesQueryHandler(_dbContext.Object);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        var companiesList = result.ToList();
        Assert.Empty(companiesList);
    }
}