namespace API.Auth;

public class TokenOptions
{
    public string SigningKey { get; set; } = default!;
    public string Audience { get; set; } = default!;
    public string Issuer { get; set; } = default!;
    public bool ValidateSigningKey { get; set; }
    public int AccessTokenLifetimeInMinutes { get; set; }
}
