using Microsoft.AspNetCore.Mvc;
using BackEndApi.Models.User;
using BackEndApi.Database;
using BackEndApi.Services;
using System.Threading.Tasks;

[ApiController]
[Route("/api/v1/[controller]")]
public class AuthController : ControllerBase
{
    private readonly PostgresDatabase _db;

    public AuthController(PostgresDatabase db)
    {
        _db = db;
    }

    [HttpPost("registernew")]
    public async Task<IActionResult> RegisterNewUser([FromBody] UserCreateForm user)
    {
        if (user == null || string.IsNullOrWhiteSpace(user.Name) ||
            string.IsNullOrWhiteSpace(user.Surname) || string.IsNullOrWhiteSpace(user.Secret) ||
            string.IsNullOrWhiteSpace(user.Mail))
        {
            return BadRequest(new { error = true, message = "Invalid registration data" });
        }

        string hash = PasswordHash.HashPassword(user.Secret);

        var success = await _db.ExecuteAsync(@"
            INSERT INTO users (name, fullname, secret, mail, createdat, role)
            VALUES (@Nickname, @Fullname, @Secret, @Mail, CURRENT_DATE, 'User')",
            new
            {
                Nickname = user.Name,
                Fullname = $"{user.Name} {user.Surname}",
                Secret   = hash,
                Mail     = user.Mail
            });

        return success
            ? Ok(new { result = true })
            : BadRequest(new { error = true, message = "Registration failed" });
    }
}