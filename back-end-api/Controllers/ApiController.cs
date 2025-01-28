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
	public class ApiController : ControllerBase
	{
		[HttpPost("authuser")]
		public IActionResult AuthUser ([FromBody] User user)
		{
			if(user is null)
			{
				return BadRequest("Data not informed on body");
			}

			return Ok("");
		}
	}
}