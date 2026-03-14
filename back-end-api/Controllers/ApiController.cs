using Microsoft.AspNetCore.Mvc;
using BackEndApi.Models.User;
using BackEndApi.Database;
using BackEndApi.Services;
using System.Threading.Tasks;

[ApiController]
[Route("api/v1")]
public class ApiController : ControllerBase
{
    private readonly PostgresDatabase _db;

    public ApiController(PostgresDatabase db)
    {
        _db = db;
    }

    [HttpGet("time")]
    public IActionResult Time()
    {
        return Ok(new { time = DateTime.Now.ToString("HH\\hmm") });
    }

    [HttpPost("token")]
    public async Task<IActionResult> GetUserToken([FromBody] User user)
    {
        if (user == null || string.IsNullOrWhiteSpace(user.Name) || string.IsNullOrWhiteSpace(user.Secret))
        {
            return BadRequest(new { error = true, message = "Invalid credentials" });
        }

        var rows = await _db.QueryAsync<User>(
            "SELECT * FROM users WHERE name = @Name",
            new { Name = user.Name }
        );

        if (rows.Length == 0)
        {
            return BadRequest(new { error = true, message = "User not found" });
        }

        var dbUser = rows[0];

        if (!PasswordHash.BalancePasswords(user.Secret, dbUser.Secret))
        {
            return BadRequest(new { error = true, message = "Invalid credentials" });
        }

        var token = JwtToken.GetToken(dbUser);

        return Ok(new { error = false, token });
    }
}