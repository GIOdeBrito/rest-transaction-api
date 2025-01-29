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
		[HttpGet("db-test")]
		public IActionResult DbTest ()
		{
			PostgresDatabase db = new ();

			User[] rows = db.Query<User>("SELECT * FROM USERS");

			foreach(User row in rows.ToArray())
			{
				Console.WriteLine($"Name: {row.Name} | Mail: {row.Mail}");
			}

			return Ok("Done");
		}

		[HttpGet("db-test-bind")]
		public IActionResult DbTestBind ()
		{
			PostgresDatabase db = new ();

			object queryParams = new {
				Id = 2
			};

			User[] rows = db.Query<User>("SELECT * FROM USERS WHERE ID = @Id", queryParams);

			return Ok($"ID {rows[0].Id} name: {rows[0].Name}");
		}

		[HttpPost("user-auth")]
		[Authorize("User")]
		public IActionResult UserAuthTest ()
		{
			User user = new User();

			string token = JwtToken.GetToken(user);

			return Ok(token);
		}
	}
}