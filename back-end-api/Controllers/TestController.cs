using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BackEndApi.Models;
using BackEndApi.Database;

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
		public IActionResult JToken ([FromBody] Models.User user)
		{
			object json = new
			{
				message = "Accepted!"
			};

			return Ok(json);
		}

		[HttpGet("tdb")]
		public IActionResult DbTest ()
		{
			DatabaseConnection db = new ();

			db.Query("SELECT * FROM USERS");

			return Ok("done");
		}
	}
}