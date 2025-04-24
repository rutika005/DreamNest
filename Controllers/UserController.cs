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

        public object JsonRequestBehavior { get; private set; }

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
            HttpContext.Session.Clear(); // Session deleted
            return RedirectToAction("Index", "Home"); // return toguest panel
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

        public IActionResult Budget(int propertyId)
        {
            var property = _context.properties.FirstOrDefault(p => p.PropertyId == propertyId);
            if (property == null)
            {
                return NotFound();
            }

            var model = new PaymentViewModel
            {
                PropertyId = property.PropertyId.ToString(),
                PropertyTitle = property.Title,
                PropertyLocation = property.Address,
                RentAmount = property.Price
            };

            return View(model);
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

        public JsonResult GetJobDetails(int id)
        {
            var job = _context.JobListings.FirstOrDefault(j => j.Id == id);
            if (job == null)
                return Json(null);

            return Json(new
            {
                title = job.Title,
                location = job.Location,
                jobType = job.JobType,
                experience = job.Experience
            });
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
