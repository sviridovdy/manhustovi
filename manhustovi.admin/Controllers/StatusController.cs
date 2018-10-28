using Microsoft.AspNetCore.Mvc;

namespace manhustovi.admin.Controllers
{
	[Route("api/[controller]")]
	public class StatusController : Controller
	{
		[HttpGet]
		public IActionResult Get()
		{
			return Content("Hello");
		}
	}
}