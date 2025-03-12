using System.Diagnostics;
using Aesthetica.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Aesthetica.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;  // Database context instance
        private readonly EmailService _emailService;  // Email service instance

        public HomeController(ILogger<HomeController> logger, AppDbContext context, EmailService emailService)
        {
            _logger = logger;
            _context = context;
            _emailService = emailService;
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

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactModel model)
        {
            if (ModelState.IsValid)
            {
                _context.contactus.Add(model);  // Insert into database
                _context.SaveChanges();
                ViewBag.Message = "Your message has been sent successfully!";
                return View("Contact");
            }
            return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // ✅ Random token generate karo
                model.token = Guid.NewGuid().ToString(); // Unique token generate karega
                model.IsVerified = false; // Default false
                Console.WriteLine("Generated Token: " + model.token);

                // ✅ Save to Database
                _context.registeruser.Add(model);
                _context.SaveChanges();

                // ✅ Prepare Verification Link
                string verificationLink = Url.Action("VerifyEmail", "Home", new { model.token }, Request.Scheme);

                // ✅ Email Body
                string emailBody = $"<h2>Welcome, {model.Name}!</h2><p>Please verify your email by clicking <a href='{verificationLink}'>here</a>.</p>";

                // ✅ Prepare replacements dictionary
                var replacements = new Dictionary<string, string>
                {
                    { "UserName", model.Name },
                    { "VerificationLink", verificationLink }
                };

                // ✅ Send Email
                _emailService.SendEmail(model.Id, "Verify Your Email", emailBody, replacements);

                TempData["Message"] = "Registered successfully! Check your email to verify your account.";
                return RedirectToAction("Register", "Home");
            }
            return View(model);
        }


        public IActionResult VerifyEmail(string token)
        {
            var user = _context.registeruser.FirstOrDefault(u => u.token == token);
            if (user != null)
            {
                user.IsVerified = true;
                user.token = null; // Token null kar do, taki dobara use na ho
                _context.SaveChanges();
                TempData["Message"] = "Email verified successfully!";
            }
            else
            {
                TempData["Message"] = "Invalid or expired token.";
            }
            return RedirectToAction("Index");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
