using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace RedRiverTest.Api.Auth;

public interface IJwtTokenService
{
    // Create a JWT token for a user.
	string CreateToken(string username);
}

public sealed class JwtTokenService : IJwtTokenService
{
   // Builds JWT tokens used by the API.
	private readonly JwtOptions _options;

	public JwtTokenService(IOptions<JwtOptions> options)
	{
		_options = options.Value;
	}

	public string CreateToken(string username)
	{
        // We keep a few claims in the token.
		var claims = new List<Claim>
		{
			new(JwtRegisteredClaimNames.Sub, username),
			new(JwtRegisteredClaimNames.UniqueName, username),
			new(ClaimTypes.Name, username)
		};

        // Use the secret key from config.
		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SigningKey));
		var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

       // Create the token.
		var token = new JwtSecurityToken(
			issuer: _options.Issuer,
			audience: _options.Audience,
			claims: claims,
			expires: DateTime.UtcNow.AddMinutes(_options.ExpirationMinutes),
			signingCredentials: creds);

		return new JwtSecurityTokenHandler().WriteToken(token);
	}
}
