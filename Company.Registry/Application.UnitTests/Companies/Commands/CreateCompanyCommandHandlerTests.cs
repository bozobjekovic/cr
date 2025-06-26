using Application.Abstractions.Data;
using Application.Companies.Create;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Application.UnitTests.Companies.Commands;

public class CreateCompanyCommandHandlerTests
{
    private readonly Mock<IApplicationDbContext> _dbContext = new();
    private readonly Mock<DbSet<Company>> _companiesDbSet = new();

    [Fact]
    public async Task Handle_ShouldCreateCompany_WhenCommandIsValid()
    {
        // Arrange
        var command = new CreateCompanyCommand("Company", "Exchange", "Ticker", "US0378331005");

        _dbContext.Setup(x => x.Companies).Returns(_companiesDbSet.Object);
        _dbContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var handler = new CreateCompanyCommandHandler(_dbContext.Object);

        // Act
        var companyDto = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(companyDto);
        Assert.Equal(command.Name, companyDto.Name);
        Assert.Equal(command.Exchange, companyDto.Exchange);

        _companiesDbSet.Verify(x => x.Add(It.IsAny<Company>()), Times.Once);

        _dbContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}