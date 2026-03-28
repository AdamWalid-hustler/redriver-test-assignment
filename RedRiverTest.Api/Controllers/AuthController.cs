using Microsoft.AspNetCore.Mvc;
using RedRiverTest.Api.Auth;
using RedRiverTest.Api.Contracts;

namespace RedRiverTest.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController : ControllerBase
{
	private readonly IUserStore _users;
	private readonly IJwtTokenService _tokens;

	public AuthController(IUserStore users, IJwtTokenService tokens)
	{
		_users = users;
		_tokens = tokens;
	}

	[HttpPost("register")]
	public IActionResult Register(RegisterRequest request)
	{
		if (!_users.TryRegister(request.Username, request.Password))
		{
			return BadRequest("Registration failed.");
		}

		return NoContent();
	}

	[HttpPost("login")]
	public ActionResult<AuthResponse> Login(LoginRequest request)
	{
		if (!_users.ValidateCredentials(request.Username, request.Password))
		{
			return Unauthorized();
		}

		var token = _tokens.CreateToken(request.Username);
		return Ok(new AuthResponse(token));
	}
}
