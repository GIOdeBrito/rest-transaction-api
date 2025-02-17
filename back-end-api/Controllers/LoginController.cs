using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using BackEndApi.Models.User;
using BackEndApi.Services;
using BackEndApi.Database;

namespace BackEndApi.Controllers
{
	[ApiController]
	[Route("/api/v1/[controller]")]
	public class LoginController : Controller
	{
		[HttpPost("login")]
		public IActionResult LoginUser ([FromBody] User user)
		{
			if(user is null)
			{
				return BadRequest(new { error = true, result = "Malformed form data." });
			}

			object queryParams = new {
				Name = user.Name
			};

			PostgresDatabase db = new ();

			User[] rows = db.Query<User>("SELECT * FROM USERS WHERE NAME = @Name", queryParams);

			// No such user, then returns to login page
			if(rows.Length == 0)
			{
				return BadRequest(new { error = true, result = "User not found." });
			}

			// Gets the matched user
			User row = rows[0];

			// Checks whether the passwords are also a match
			if(!PasswordHash.BalancePasswords(user.Secret, row.Secret))
			{
				return BadRequest(new { error = true, result = "Login or password does not match." });
			}

			int Id = row.Id ?? -1;
			string Name = row.Name;
			string FullName = row.Fullname;
			string CreatedAt = row.CreatedAt.ToString();
			string Role = row.Role;

			object response = new {
				id = Id,
				name = Name,
				fullname = FullName,
				createdat = CreatedAt,
				role = Role
			};

			return Ok(response);
		}
	}
}