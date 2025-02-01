using System;
using Microsoft.AspNetCore.Mvc;
using BackEndApi.Models;

namespace BackEndApi.Controllers
{
	[ApiController]
	[Route("/api/v1/[controller]")]
	public class AuthController : Controller
	{
		[HttpPost("login")]
		public IActionResult LoginUser ([FromForm] User user)
		{
			return Ok();
		}
	}
}