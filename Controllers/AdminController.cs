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
    }
}
