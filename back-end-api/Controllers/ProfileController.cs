using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace BackEndApi.Controllers
{
	[ApiController]
	[Route("/")]
	public class ProfileController : Controller
	{
		[HttpGet("profile")]
		public IActionResult Index ()
		{
			if(string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
			{
				return LocalRedirect("~/login");
			}

			string Username = HttpContext.Session.GetString("UserName");
			string Role = HttpContext.Session.GetString("UserRole");
			string Created = HttpContext.Session.GetString("UserCreated");

			ViewData["Title"] = Username + "'s Profile";
			ViewData["Username"] = Username;
			ViewData["Role"] = Role;
			ViewData["CreationTime"] = Created;

			return View();
		}
	}
}