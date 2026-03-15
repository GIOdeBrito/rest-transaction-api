using Microsoft.AspNetCore.Mvc;
using BackEndApi.DTO.User;
using BackEndApi.Interfaces;
using BackEndApi.Services;
using System.Threading.Tasks;

[ApiController]
[Route("/api/v1/[controller]")]
public class LoginController : ControllerBase
{
	private readonly IUserRepository _userRepo;

	public LoginController(IUserRepository userRepo)
	{
		_userRepo = userRepo;
	}

	[HttpPost("login")]
	public async Task<IActionResult> LoginUser([FromBody] User user)
	{
		if(user == null || string.IsNullOrWhiteSpace(user.Name) || string.IsNullOrWhiteSpace(user.Secret))
		{
			return BadRequest(new { error = true, message = "Invalid credentials" });
		}

		bool isUser = await _userRepo.IsUserByNameAsync(user.Name);

		if(!isUser)
		{
			return BadRequest(new { error = true, message = "User not found" });
		}

		var dbUser = await _userRepo.GetUserByNameAsync(user.Name);

		if(dbUser == null || !PasswordHash.Verify(user.Secret, dbUser.Secret))
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