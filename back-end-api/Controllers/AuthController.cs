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
	public class AuthController : Controller
	{
		[HttpPost("login")]
		public IActionResult LoginUser ([FromForm] User user)
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
				//return LocalRedirect("~/login");
				return BadRequest(new { error = true, result = "User not found." });
			}

			// Gets the matched user
			User row = rows[0];

			// Checks whether the passwords are also a match
			if(!PasswordHash.BalancePasswords(user.Secret, row.Secret))
			{
				//return LocalRedirect("~/login");
				return BadRequest(new { error = true, result = "Login or password does not match." });
			}

			int Id = row.Id ?? 0;
			string Name = row.Name;
			string FullName = row.Fullname;
			string CreatedAt = row.CreatedAt.ToString();
			string Role = row.Role;

			HttpContext.Session.SetInt32("UserId", Id);
			HttpContext.Session.SetString("UserName", Name);
			HttpContext.Session.SetString("UserFullName", FullName);
			HttpContext.Session.SetString("UserCreated", CreatedAt);
			HttpContext.Session.SetString("UserRole", Role);

			return Ok("~/");
		}

		[HttpPost("logoff")]
		public IActionResult LogoffUser ()
		{
			HttpContext.Session.Clear();

			return LocalRedirect("~/");
		}

		[HttpPost("registernew")]
		public IActionResult RegisterNewUser ([FromForm] UserCreateForm user)
		{
			if(user is null)
			{
				return BadRequest(new { error = true, result = "Malformed form data." });
			}

			object nonQueryParams = new {
				Nickname = user.Name,
				Fullname = $"{user.Name} {user.Surname}",
				Secret = user.Secret,
				Mail = user.Mail
			};

			string sql = @"
				INSERT INTO
					USERS
					(NAME, FULLNAME, SECRET, MAIL, CREATEDAT, ROLE)
				VALUES
					(@Nickname, @Fullname, @Secret, @Mail, CURRENT_DATE, 'User');
			";

			PostgresDatabase db = new ();

			bool success = db.Execute(sql, nonQueryParams);

			if(!success)
			{
				Console.WriteLine("Error. Could not insert data into database.");
				return LocalRedirect("~/signin");
			}

			return LocalRedirect("~/login");
		}
	}
}