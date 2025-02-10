using System;
using Microsoft.AspNetCore.Mvc;

namespace BackEndApi.Controllers
{
	[ApiController]
	[Route("/")]
	public class SignInController : Controller
	{
		[HttpGet("signin")]
		public IActionResult Index ()
		{
			ViewData["Title"] = "Sign in";

			return View();
		}
	}
}