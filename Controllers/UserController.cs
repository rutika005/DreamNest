using Microsoft.AspNetCore.Mvc;

namespace Aesthetica.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            // ✅ Check agar user logged in hai
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // ✅ Session delete kar do
            return RedirectToAction("Index", "Home"); // ✅ Wapas guest panel dikhao
        }

    }
}
