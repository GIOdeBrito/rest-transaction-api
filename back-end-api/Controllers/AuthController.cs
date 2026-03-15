using Microsoft.AspNetCore.Mvc;
using BackEndApi.DTO.User;
using BackEndApi.Interfaces;
using BackEndApi.Services;
using System.Threading.Tasks;

[ApiController]
[Route("/api/v1/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _userRepo;

    public AuthController (IUserRepository userRepo)
    {
        _userRepo = userRepo;
    }

    [HttpPost("registernew")]
    public async Task<IActionResult> RegisterNewUser([FromBody] UserCreateForm user)
    {
        if(user == null || string.IsNullOrWhiteSpace(user.Name)
		|| string.IsNullOrWhiteSpace(user.Surname) || string.IsNullOrWhiteSpace(user.Secret)
		|| string.IsNullOrWhiteSpace(user.Mail))
        {
            return BadRequest(new { error = true, message = "Invalid registration data" });
        }

        string hash = PasswordHash.Hash(user.Secret);
		bool success = await _userRepo.CreateAsync(user, hash);

        return success
            ? Ok(new { result = true })
            : BadRequest(new { error = true, message = "Registration failed" });
    }
}