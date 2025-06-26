using Application.Common.Exceptions;
using Application.Companies.Create;
using Application.Companies.GetById;
using Application.Companies.GetByIsin;
using Application.Companies.Update;
using Application.IntegrationTests.Infrastructure;

namespace Application.IntegrationTests.Companies;

public class CompanyTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Create_ShouldAddCompany_WhenCommandIsValid()
    {
        // Arrange
        var command = new CreateCompanyCommand("Company", "Exchange", "Ticker", "US0378331005");

        // Act
        var companyDto = await Sender.Send(command);

        // Assert
        var company = DbContext.Companies.FirstOrDefault(x => x.Id == companyDto.Id);

        Assert.NotNull(company);
    }

    [Fact]
    public async Task Create_ShouldNotAddCompany_WhenIsinIsInvalid()
    {
        // Arrange
        var command = new CreateCompanyCommand("Company", "Exchange", "Ticker", "0378331005");

        // Act
        Task Action() => Sender.Send(command);

        // Assert
        await Assert.ThrowsAsync<ValidationAppException>(Action);
    }

    [Fact]
    public async Task Create_ShouldNotAddCompany_WhenThereIsAlreadyACompanyWithTheSameIsin()
    {
        // Arrange
        var command = new CreateCompanyCommand("Company", "Exchange", "Ticker", "US0378331029");
        await Sender.Send(command);

        var newCommand = new CreateCompanyCommand("Company2", "Exchange2", "Ticker2", "US0378331029");

        // Act
        Task Action() => Sender.Send(newCommand);

        // Assert
        await Assert.ThrowsAsync<ValidationAppException>(Action);
    }

    [Fact]
    public async Task Update_ShouldUpdateCompany_WhenCommandIsValid()
    {
        // Arrange
        var company = new CreateCompanyCommand("Company", "Exchange", "Ticker", "US0378331005");
        var companyDto = await Sender.Send(company);

        var updateCompany =
            new UpdateCompanyCommand(companyDto.Id, "Company2", "Exchange2", "Ticker2", "US0378331007", null);

        // Act
        var result = await Sender.Send(updateCompany);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(updateCompany.Name, result.Name);
        Assert.Equal(updateCompany.Exchange, result.Exchange);
    }

    [Fact]
    public async Task GetById_ShouldReturnCompany_WhenCompanyExists()
    {
        // Arrange
        var company = new CreateCompanyCommand("Company", "Exchange", "Ticker", "US0378331099");
        var companyDto = await Sender.Send(company);

        // Act
        var result = await Sender.Send(new GetCompanyByIdQuery(companyDto.Id));

        // Assert
        Assert.NotNull(result);
        Assert.Equal(companyDto.Id, result.Id);
        Assert.Equal(companyDto.Name, result.Name);
    }

    [Fact]
    public async Task GetById_ShouldThrowException_WhenCompanyDoesNotExist()
    {
        // Arrange
        var command = new GetCompanyByIdQuery(Guid.NewGuid());

        // Act
        Task Action() => Sender.Send(command);

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(Action);
    }

    [Fact]
    public async Task GetByIsin_ShouldReturnCompany_WhenCompanyExists()
    {
        // Arrange
        var company = new CreateCompanyCommand("Company", "Exchange", "Ticker", "US0378338705");
        var companyDto = await Sender.Send(company);

        // Act
        var result = await Sender.Send(new GetCompanyByIsinQuery("US0378338705"));

        // Assert
        Assert.NotNull(result);
        Assert.Equal(companyDto.Id, result.Id);
        Assert.Equal(companyDto.Name, result.Name);
    }

    [Fact]
    public async Task GetByIsin_ShouldThrowException_WhenCompanyDoesNotExist()
    {
        // Arrange
        var command = new GetCompanyByIsinQuery("US0378341005");

        // Act
        Task Action() => Sender.Send(command);

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(Action);
    }

    [Fact]
    public async Task GetByIsin_ShouldThrowException_WhenIsinIsInvalid()
    {
        // Arrange
        var command = new GetCompanyByIsinQuery("0378331005");

        // Act
        Task Action() => Sender.Send(command);

        // Assert
        await Assert.ThrowsAsync<ValidationAppException>(Action);
    }
}