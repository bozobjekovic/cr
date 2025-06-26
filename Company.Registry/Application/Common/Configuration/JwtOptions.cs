namespace Application.Common.Configuration;

public class JwtOptions
{
    public string Authority { get; init; } = string.Empty;
    
    public string Audience { get; init; } = string.Empty;
}