using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ProjectBig.Models;
using System.Collections.Generic;

namespace ProjectBig.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ItemController _itemController;

        public HomeController(ILogger<HomeController> logger, ItemController itemController)
        {
            _logger = logger;
            _itemController = itemController;
        }

        public IActionResult Index()
        {
            // Call the Index action method of the ItemController
            IActionResult result = _itemController.Index();

            // Check if the result is a ViewResult
            if (result is ViewResult viewResult)
            {
                // Pass the model from the ItemController's Index view to the Home/Index view
                return View(viewResult.ViewName, viewResult.Model);
            }

            // If the result is not a ViewResult, return the result as is
            return result;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
