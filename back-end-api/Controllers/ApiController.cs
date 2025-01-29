using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BackEndApi.Models;
using BackEndApi.Database;
using BackEndApi.Services;

namespace BackEndApi.Controllers
{
	[ApiController]
	[Route("api/v1")]
	public class ApiController : ControllerBase
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

		[HttpPost("auth")]
		public IActionResult AuthUser ([FromBody] User user)
		{
			if(user is null)
			{
				return BadRequest(new { error = true, result = "Malformed data on body." });
			}

			object queryParams = new {
				Name = user.Name,
				Secret = user.Secret
			};

			PostgresDatabase db = new ();

			User[] rows = db.Query<User>("SELECT * FROM USERS WHERE NAME = @Name AND SECRET = @Secret", queryParams);

			if(rows.Length == 0)
			{
				return BadRequest(new { error = true, result = "Error! User or credentials are incorrect." });
			}

			user.Role = rows[0].Role;

			string token = JwtToken.GetToken(user);

			return Ok(new { error = false, result = token });
		}
	}
}