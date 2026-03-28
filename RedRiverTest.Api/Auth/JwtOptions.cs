namespace RedRiverTest.Api.Auth;

public sealed class JwtOptions
{
	public const string SectionName = "Jwt";

 // Who made the token.
	public string Issuer { get; init; } = string.Empty;
   // Who the token is for.
	public string Audience { get; init; } = string.Empty;
 // Secret key used to sign tokens.
	public string SigningKey { get; init; } = string.Empty;
   // How long the token should last.
	public int ExpirationMinutes { get; init; } = 60;
}
