using System;
using Microsoft.AspNetCore.Mvc;

namespace BackEndApi.Controllers
{
	[ApiController]
	[Route("/")]
	public class LoginController : Controller
	{
		[HttpGet("login")]
		public IActionResult Index ()
		{
			ViewData["Title"] = "Login";

			return View();
		}
	}
}