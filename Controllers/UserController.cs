using Microsoft.AspNetCore.Mvc;

namespace Aesthetica.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Session deleted
            return RedirectToAction("Index", "Home"); // return toguest panel
        }

        public IActionResult Blog()
        {
            return View();
        }

        public IActionResult Style()
        {
            return View();
        }

        public IActionResult Budget()
        {
            return View();
        }

        public IActionResult Measure()
        {
            return View();
        }

        public IActionResult Career()
        {
            return View();
        }
    }
}
