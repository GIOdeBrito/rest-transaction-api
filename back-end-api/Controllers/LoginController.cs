using Microsoft.AspNetCore.Mvc;
using BackEndApi.Models.User;
using BackEndApi.Database;
using BackEndApi.Services;
using System.Threading.Tasks;

[ApiController]
[Route("/api/v1/[controller]")]
public class LoginController : ControllerBase
{
	private readonly PostgresDatabase _db;

	public LoginController(PostgresDatabase db)
	{
		_db = db;
	}

	[HttpPost("login")]
	public async Task<IActionResult> LoginUser([FromBody] User user)
	{
		if(user == null || string.IsNullOrWhiteSpace(user.Name) || string.IsNullOrWhiteSpace(user.Secret))
		{
			return BadRequest(new { error = true, message = "Invalid credentials" });
		}

		var rows = await _db.QueryAsync<User>(
			"SELECT * FROM users WHERE name = @Name",
			new { Name = user.Name }
		);

		if(rows.Length == 0)
		{
			return BadRequest(new { error = true, message = "User not found" });
		}

		var dbUser = rows[0];

		if(!PasswordHash.BalancePasswords(user.Secret, dbUser.Secret))
		{
			return BadRequest(new { error = true, message = "Invalid credentials" });
		}

		var response = new {
			id        = dbUser.Id ?? -1,
			name      = dbUser.Name,
			fullname  = dbUser.Fullname,
			createdat = dbUser.CreatedAt?.ToString("o"),
			role      = dbUser.Role
		};

		return Ok(response);
	}
}