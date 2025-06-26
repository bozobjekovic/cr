using FluentValidation;

namespace Application.Companies.GetByIsin;

public class GetCompanyByIsinQueryValidator : AbstractValidator<GetCompanyByIsinQuery>
{
    public GetCompanyByIsinQueryValidator()
    {
        RuleFor(x => x.Isin).NotEmpty()
            .Matches(@"^[A-Z]{2}[A-Z0-9]{10}$")
            .WithMessage("ISIN must be 12 characters long and start with two uppercase letters");
    }
}