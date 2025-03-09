using System.Diagnostics;
using Aesthetica.Models;
using Microsoft.AspNetCore.Mvc;

namespace Aesthetica.Controllers
{
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

        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult Blog()
        {
            return View();
        }

        public IActionResult Gallery()
        {
            return View();
        }

        public IActionResult Career()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Contact()
        {
            return View(new ContactModel()); // Ensure model binding
        }

        [HttpPost]
        public IActionResult Contact(ContactModel model)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Message = "Your message has been sent successfully!";
                return View(new ContactModel()); // Clear fields after submission
            }
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
