using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BackEndApi.Models;
using BackEndApi.Database;
using BackEndApi.Services;

namespace BackEndApi.Controllers
{
	[ApiController]
	[Route("api/v1/[controller]")]
	public class TestController : ControllerBase
	{
		[HttpGet("time")]
		public IActionResult Time ()
		{
			object json = new
			{
				time = DateTime.Now.ToString("HH\\hmm", System.Globalization.DateTimeFormatInfo.InvariantInfo)
			};

			return Ok(json);
		}

		[HttpGet("auth-test")]
		[Authorize]
		public IActionResult GetJson ([FromBody] User user)
		{
			object json = new
			{
				message = "Accepted!"
			};

			return Ok(json);
		}

		[HttpGet("db-test")]
		public IActionResult DbTest ()
		{
			DatabaseConnection db = new ();

			List<User> rows = db.Query<User>("SELECT * FROM USERS");

			foreach(var row in rows.ToArray())
			{
				Console.WriteLine($"Name: {row.Name} | Mail: {row.Mail}");
			}

			return Ok("Done");
		}

		[HttpGet("get-token")]
		public IActionResult GetToken ()
		{
			User user = new User();

			string token = JwtToken.GetToken(user);

			return Ok(token);
		}
	}
}