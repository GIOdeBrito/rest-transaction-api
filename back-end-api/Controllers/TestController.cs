using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BackEndApi.DTO.User;
using BackEndApi.Database;
using BackEndApi.Services;
using System.Threading.Tasks;

[ApiController]
[Route("api/v1/[controller]")]
public class TestController : ControllerBase
{
    private readonly PostgresDatabase _db;

    public TestController(PostgresDatabase db)
    {
        _db = db;
    }

    [HttpGet("db-test")]
    public async Task<IActionResult> DbTest()
    {
        var rows = await _db.QueryAsync<User>("SELECT * FROM users");

        foreach (var row in rows)
        {
            Console.WriteLine($"Name: {row.Name} | Mail: {row.Mail}");
        }

        return Ok("Done");
    }

    [HttpGet("db-test-bind")]
    public async Task<IActionResult> DbTestBind()
    {
        var rows = await _db.QueryAsync<User>(
            "SELECT * FROM users WHERE id = @Id",
            new { Id = 2 }
        );

        if (rows.Length == 0) return NotFound();

        var user = rows[0];
        return Ok($"ID {user.Id} name: {user.Name}");
    }

    [HttpPost("user-auth")]
    [Authorize(Roles = "User")]
    public IActionResult UserAuthTest()
    {
        return Ok("Allowed");
    }

    [HttpGet("hashpwd")]
    public IActionResult HashPwd([FromQuery(Name = "pwd")] string password)
    {
        if (string.IsNullOrEmpty(password)) return BadRequest("Password required");

        string hash = PasswordHash.Hash(password);
        return Ok(hash);
    }
}