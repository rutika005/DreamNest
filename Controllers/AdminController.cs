using Microsoft.AspNetCore.Mvc;

namespace Aesthetica.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("UserEmail") != "admin")
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        public IActionResult Blog()
        {
            return View();
        }

        public IActionResult Budget()
        {
            return View();
        }

        public IActionResult Room()
        {
            return View();
        }

        public IActionResult Career()
        {
            return View();
        }

        public IActionResult Setting()
        {
            return View();
        }
    }
}
