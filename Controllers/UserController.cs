using Aesthetica.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations.Schema;


namespace Aesthetica.Controllers
{
    public class UserController : Controller
    {

        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        [RoleAuthorize("User")]
        public async Task<IActionResult> Index()
        {
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); 
            return RedirectToAction("Index", "Home"); 
        }

        public IActionResult Blog()
        {
            var savedPosts = _context.savedposts.OrderByDescending(p => p.SavedAt).ToList();
            return View(savedPosts);
        }

        [HttpPost("save-post")]
        public IActionResult SavePost([FromBody] SavedPost post)
        {
            var savedPosts = _context.savedposts.OrderByDescending(p => p.SavedAt).ToList();
            if (ModelState.IsValid)
            {
                post.SavedAt = DateTime.Now;
                _context.savedposts.Add(post);
                _context.SaveChanges();
                return Ok(new { success = true });
            }

            return View(savedPosts);
        }

        public IActionResult Style()
        {
            return View();
        }

        public IActionResult Budget()
        {
            //var model = new PaymentViewModel
            //{
            //    // Populate dummy/test values to avoid nulls
            //    UserId=1,
            //    PropertyID = 1,
            //    Amount = 0
            //    // Add other required properties here
            //};
            return View();
        }

        public IActionResult SavedDesign()
        {
            var savedPosts = _context.savedposts
        .Where(p => p.SavedAt != null)
        .OrderByDescending(p => p.SavedAt)
        .ToList();

            return View(savedPosts);
        }
        public IActionResult Measure()
        {
            return View();
        }

        public IActionResult Career()
        {
            return View();
        }

        public IActionResult Profile()
        {
            var model = _context.userRegister.FirstOrDefault();

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }
    }
}
