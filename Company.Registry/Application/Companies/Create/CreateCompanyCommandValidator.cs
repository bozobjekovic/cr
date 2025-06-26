using Application.Abstractions.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Create;

public class CreateCompanyCommandValidator : AbstractValidator<CreateCompanyCommand>
{
    public CreateCompanyCommandValidator(IApplicationDbContext dbContext)
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Exchange).NotEmpty();
        RuleFor(x => x.Ticker).NotEmpty();
        RuleFor(x => x.Isin).NotEmpty()
            .Matches(@"^[A-Z]{2}[A-Z0-9]{10}$")
            .WithMessage("Isin must be 12 characters long and start with two uppercase letters")
            .MustAsync(async (isin, cancellationToken) =>
            {
                return !await dbContext.Companies.AnyAsync(c => c.Isin == isin, cancellationToken);
            })
            .WithMessage("Isin already specified");
    }
}