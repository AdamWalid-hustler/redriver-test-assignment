using System.Collections.Concurrent;

namespace RedRiverTest.Api.Auth;

public interface IUserStore
{
 // Add a new user.
	bool TryRegister(string username, string password);
 // Check if username + password is correct.
	bool ValidateCredentials(string username, string password);
}

public sealed class InMemoryUserStore : IUserStore
{
   // Simple user store for testing.
	// Note: data is lost when the app restarts.
	private readonly ConcurrentDictionary<string, string> _users = new(StringComparer.OrdinalIgnoreCase);

	public bool TryRegister(string username, string password)
	{
     // Basic checks.
		if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
		{
			return false;
		}

		return _users.TryAdd(username.Trim(), password);
	}

	public bool ValidateCredentials(string username, string password)
	{
     // Basic checks.
		if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
		{
			return false;
		}

		return _users.TryGetValue(username.Trim(), out var stored) && stored == password;
	}
}
