using Microsoft.AspNetCore.Mvc;

namespace Calling.Controllers
{
	public class DashboardController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}