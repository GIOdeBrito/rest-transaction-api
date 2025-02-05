using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace BackEndApi.Controllers
{
	[ApiController]
	[Route("/")]
	public class AboutController : Controller
	{
		[HttpGet("about")]
		public IActionResult Index ()
		{
			if(string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
			{
				return LocalRedirect("~/login");
			}

			ViewData["Title"] = "About";

			return View();
		}
	}
}