using Microsoft.AspNetCore.Mvc;

namespace Aesthetica.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
