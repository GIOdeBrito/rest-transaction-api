using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace BackEndApi.Controllers
{
	[ApiController]
	[Route("/")]
	public class HomeController : Controller
	{
		[HttpGet]
		public IActionResult Index ()
		{
			if(string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
			{
				return LocalRedirect("~/login");
			}

			ViewData["Title"] = "Home";
			ViewData["Name"] = HttpContext.Session.GetString("UserName");

			return View();
		}
	}
}