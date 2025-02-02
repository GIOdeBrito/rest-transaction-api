using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using BackEndApi.Models;
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
				Name = user.Name,
				Secret = user.Secret
			};

			PostgresDatabase db = new ();

			User[] rows = db.Query<User>("SELECT * FROM USERS WHERE NAME = @Name AND SECRET = @Secret", queryParams);

			// No such user, returns to login page
			if(rows.Length == 0)
			{
				return LocalRedirect("~/login");
			}

			int Id = rows[0].Id ?? 0;
			string Name = rows[0].Name;

			HttpContext.Session.SetInt32("UserId", Id);
			HttpContext.Session.SetString("UserName", Name);

			return LocalRedirect("~/");
		}

		[HttpPost("logoff")]
		public IActionResult LogoffUser ()
		{
			HttpContext.Session.Clear();

			return LocalRedirect("~/");
		}
	}
}