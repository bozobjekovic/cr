using Application.Abstractions.Data;
using Application.Companies.Update;
using Domain.Entities;
using MockQueryable.Moq;
using Moq;

namespace Application.UnitTests.Companies.Commands;

public class UpdateCompanyCommandHandlerTests
{
    private readonly Mock<IApplicationDbContext> _dbContext = new();

    [Fact]
    public async Task Handle_ShouldUpdateCompany_WhenCommandIsValid()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var command = new UpdateCompanyCommand(
            companyId,
            "Updated Company",
            "Updated Exchange",
            "Updated Ticker",
            "US0378331007",
            "https://updated.com");

        var existingCompany = new Company
        {
            Id = companyId,
            Name = "Company",
            Exchange = "Exchange",
            Ticker = "Ticker",
            Isin = "US0378331005",
            Website = "https://original.com"
        };

        var companies = new List<Company> { existingCompany };
        var mockDbSet = companies.AsQueryable().BuildMockDbSet();

        _dbContext.Setup(x => x.Companies).Returns(mockDbSet.Object);
        _dbContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var handler = new UpdateCompanyCommandHandler(_dbContext.Object);

        // Act
        var companyDto = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(companyDto);
        Assert.Equal(command.Name, existingCompany.Name);
        Assert.Equal(command.Exchange, existingCompany.Exchange);

        mockDbSet.Verify(x => x.Update(It.IsAny<Company>()), Times.Once);
        _dbContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenCompanyNotFound()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();
        var command = new UpdateCompanyCommand(
            nonExistentId,
            "Company",
            "Exchange",
            "Ticker",
            "US0378331005",
            null);

        var mockDbSet = new List<Company>().AsQueryable().BuildMockDbSet();
        _dbContext.Setup(x => x.Companies).Returns(mockDbSet.Object);

        var handler = new UpdateCompanyCommandHandler(_dbContext.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(
            () => handler.Handle(command, CancellationToken.None));

        Assert.Equal("Company not found", exception.Message);

        mockDbSet.Verify(x => x.Update(It.IsAny<Company>()), Times.Never);
        _dbContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}