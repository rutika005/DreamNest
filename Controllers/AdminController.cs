using Aesthetica.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Aesthetica.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        [RoleAuthorize("Admin")]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("UserEmail") != "admin")
            {
                return RedirectToAction("Login", "Home");
            }

            return View();
        }

        public IActionResult Blog()
        {
            var blogPosts = _context.blogadmin.ToList();

            return View(blogPosts);
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

        public IActionResult JobApplication()
        {
            return View();
        }

        public IActionResult Setting()
        {
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Session deleted
            return RedirectToAction("Index", "Home"); // return toguest panel
        }

        [HttpPost]
        public ActionResult UpdateProfile(string FirstName, string LastName, string Email, string Bio)
        {
            // Handle profile update logic (Save to DB, validate, etc.)
            TempData["Message"] = "Profile updated successfully!";
            return RedirectToAction("Settings");
        }

        [HttpPost]
        public ActionResult UpdatePassword(string CurrentPassword, string NewPassword, string ConfirmPassword)
        {
            // Handle password update logic (validation, hashing, saving to DB)
            if (NewPassword == ConfirmPassword)
            {
                TempData["Message"] = "Password updated successfully!";
            }
            else
            {
                TempData["Error"] = "Passwords do not match!";
            }
            return RedirectToAction("Settings");
        }

        [HttpPost]
        public IActionResult Blog(BlogPost blogPost, IFormFile Thumbnail, IFormFile AuthorImage)
        {
            if (ModelState.IsValid)
            {
                // Save image files
                if (Thumbnail != null)
                {
                    var thumbnailPath = Path.Combine("wwwroot/images", Thumbnail.FileName);
                    using (var stream = new FileStream(thumbnailPath, FileMode.Create))
                    {
                        Thumbnail.CopyTo(stream);
                    }
                    blogPost.Thumbnail = Thumbnail.FileName;
                }

                if (AuthorImage != null)
                {
                    var authorImagePath = Path.Combine("wwwroot/images", AuthorImage.FileName);
                    using (var stream = new FileStream(authorImagePath, FileMode.Create))
                    {
                        AuthorImage.CopyTo(stream);
                    }
                    blogPost.AuthorImage = AuthorImage.FileName;
                }

                _context.blogadmin.Add(blogPost); // assuming _context is your DB context
                _context.SaveChanges();

                return RedirectToAction("BlogPosts");
            }

            return View("BlogPosts", _context.blogadmin.ToList());
        }

    }
}
