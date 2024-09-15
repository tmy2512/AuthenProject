using AUTHEN_PROJECT.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AUTHEN_PROJECT.Controllers
{
    //[ApiController]
    //[Route("auth")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        public IActionResult Index()
        {
            return View();
        }

        //[Route("registerView")]
        public IActionResult RegisterUser()
        {
            return View();
        }

        //[Route("loginView")]
        public IActionResult LoginUser()
        {
            // Display a success message if TempData contains the message
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"]; // Passing the message to the view via ViewBag
            }
            return View();
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
