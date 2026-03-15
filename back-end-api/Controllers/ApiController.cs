using Microsoft.AspNetCore.Mvc;
using BackEndApi.DTO.User;
using BackEndApi.Services;
using BackEndApi.Interfaces;
using System.Threading.Tasks;

[ApiController]
[Route("api/v1")]
public class ApiController : ControllerBase
{
	private readonly IUserRepository _userRepo;

    public ApiController (IUserRepository userRepo)
    {
        _userRepo = userRepo;
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

		bool userExists = await _userRepo.IsUserByNameAsync(user.Name);

        if(!userExists)
        {
            return BadRequest(new { error = true, message = "User not found" });
        }

        User? dbUser = await _userRepo.GetUserByNameAsync(user.Name);

        if(dbUser == null || !PasswordHash.Verify(user.Secret, dbUser.Secret))
        {
            return BadRequest(new { error = true, message = "Invalid credentials" });
        }

        var token = JwtToken.GetToken(dbUser);

        return Ok(new { error = false, token });
    }
}