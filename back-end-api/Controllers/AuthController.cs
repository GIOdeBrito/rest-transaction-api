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