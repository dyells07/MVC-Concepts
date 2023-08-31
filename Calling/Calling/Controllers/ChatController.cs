using Microsoft.AspNetCore.Mvc;

namespace Calling.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
