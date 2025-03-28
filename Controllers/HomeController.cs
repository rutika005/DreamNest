using System.Diagnostics;
using System.Net.Mail;
using System.Net;
using Aesthetica.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.ComponentModel.DataAnnotations;

namespace Aesthetica.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;  // Database context instance
        private readonly EmailService _emailService;  // Email service instance
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, AppDbContext context, EmailService emailService, IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            _emailService = emailService;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            //if (HttpContext.Session.GetString("Id") != null)
            //{
            //    return RedirectToAction("Index", "User");
            //}
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
                _context.contact.Add(model);  // Insert into database
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
            
                //  Random token generate karo
                model.token = Guid.NewGuid().ToString(); // Unique token generate karega
                model.IsVerified = false; // Default false
                Console.WriteLine("Generated Token: " + model.token);

                // Validate Role (Ensure it's either "User" or "Admin")
                if (model.role != "User" && model.role != "Admin")
                {
                    ModelState.AddModelError("role", "Invalid role selected.");
                    return View(model);
                }

                //  Save to Database
                _context.userregister.Add(model);
                _context.SaveChanges();

                //  Prepare Verification Link
                string verificationLink = Url.Action("VerifyEmail", "Home", new { model.token }, Request.Scheme);

                //  Email Body
                string emailBody = $"<h2>Welcome, {model.Name}!</h2><p>Please verify your email by clicking <a href='{verificationLink}'>here</a>.</p>";

                //  Prepare replacements dictionary
                var replacements = new Dictionary<string, string>
                {
                    { "{Name}", model.Name },
                    { "{VERIFY_LINK}", $"https://localhost:7150/Home/VerifyEmail?token={model.token}" }
                };

                // ✅ Send Email
                _emailService.SendEmail(model.Id, "Verify Your Email", emailBody, replacements);
                Console.WriteLine($"Name: {model.Name}, Email: {model.Email}, Token: {model.token}");


                ViewBag.Message = "Registered successfully! Check your email to verify your account.";
                return RedirectToAction("Register", "Home");
            
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel model,string email, string password)
        {
            Console.WriteLine($"Entered Email: {email}");
            Console.WriteLine($"Entered Password: {password}");

            // Check in Admin Table
            var admin = _context.admin.FirstOrDefault(a => a.Email == model.Email);

            if (admin != null)
            {
                Console.WriteLine($"Admin Password Type: {admin.Pass}");

                // Convert to string if needed
                string adminPassword = admin.Pass.Trim();

                if (admin != null && !string.IsNullOrEmpty(admin.Pass))
                {
                    Console.WriteLine("Admin login successful!");
                    HttpContext.Session.SetString("UserEmail", admin.Email);
                    HttpContext.Session.SetString("UserRole", "Admin");
                    Console.WriteLine($"Pass:  { model.Pass }");

                    return RedirectToAction("Index", "Admin"); // Redirect to Admin Panel
                }
                else
                {
                    Console.WriteLine("Incorrect admin password!");
                    ViewBag.Message = "Incorrect password!";
                    return View(model);
                }
            }

            // If email is not in admin table, check in user table
            else if (_context.userregister.Any(u => u.Email == model.Email))
            {
                var user = _context.userregister.FirstOrDefault(u => u.Email == model.Email);
                Console.WriteLine($"Stored Password from DB: {user.Pass}");

                if (user != null && !string.IsNullOrEmpty(user.Pass))
                {
                    HttpContext.Session.SetString("UserEmail", user.Email);
                    HttpContext.Session.SetString("UserRole", "User");
                    HttpContext.Session.SetString("UserName", user.Name);

                    return RedirectToAction("Index", "User");
                }
                else
                {
                    Console.WriteLine("Incorrect user password!");
                    ViewBag.Message = "Incorrect password!";
                    return View(model);
                }
            }
            else
            {
                Console.WriteLine("Email not found in database!");
                ViewBag.Message = "Email not found!";
                return View(model);
            }
        }

        public IActionResult VerifyEmail(string token)
        {
            var user = _context.userregister
            .AsEnumerable()
            .FirstOrDefault(u => !string.IsNullOrEmpty(u.token) && u.token.Equals(token, StringComparison.OrdinalIgnoreCase));

            if (user != null)
            {
                user.IsVerified = true;
                _context.SaveChanges();
                TempData["Message"] = "Email verified successfully.";
            }
            else
            {
                TempData["Message"] = "Invalid or expired token.";
            }
            return RedirectToAction("Login");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public IActionResult Login(LoginModel model)
        {
            return View(model);
        }

        public IActionResult Forgotpass()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Forgotpass(ForgotpassModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = _context.userregister.FirstOrDefault(u => u.Email == model.Email);

            if (user == null)
            {
                ViewBag.Message = "User not found.";
                return View();
            }

            // Generate a new token
            var newToken = Guid.NewGuid().ToString();

            // Overwrite the existing token and update expiry time
            user.token = newToken;
            user.ResetTokenExpiry = DateTime.UtcNow.AddHours(1);

            _context.SaveChanges();

            // ✅ Generate the reset link
            string resetLink = Url.Action("Resetpass", "Home", new { token = newToken }, Request.Scheme);

            //  Prepare replacements dictionary
            var replacements = new string[] { "{VERIFY_LINK}", $"https://localhost:7150/Home/Resetpass?token={user.token = newToken}" };

            // ✅ Call the email sending function
            await SendResetEmail(user.Email, replacements[1]);

            ViewBag.Message = "Password reset link has been sent to your email.";
            return View();
        }


        [HttpPost]
        private async Task SendResetEmail(string email, string resetLink)
        {
            var smtpClient = new SmtpClient(_configuration["EmailSettings:SMTPServer"])
            {
                Port = int.Parse(_configuration["EmailSettings:SMTPPort"]),
                Credentials = new NetworkCredential(
                _configuration["EmailSettings:SMTPUsername"],
                _configuration["EmailSettings:SMTPPassword"]
            ),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration["EmailSettings:SMTPUsername"]),
                Subject = "Reset Your Password",
                Body = $"<p>Click <a href='{resetLink}'>here</a> to reset your password.</p>",
                IsBodyHtml = true
            };

            mailMessage.To.Add(email);
            smtpClient.Send(mailMessage);
        }

        public IActionResult Resetpass()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Resetpass(ResetpassModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _context.userregister.FirstOrDefaultAsync(u => u.token == model.Token && u.ResetTokenExpiry > DateTime.UtcNow);

            if (user == null)
            {
                ViewBag.Message = "Invalid or expired token.";
                return View();
            }

            // Update the password
            _context.Entry(user).Property(u => u.Pass).IsModified = true;
            _context.Entry(user).Property(u => u.token).IsModified = true;
            //user.token = null; // Clear token
            user.ResetTokenExpiry = null;

            await _context.SaveChangesAsync();

            ViewBag.Message = "Password successfully reset! You can now login.";
            return RedirectToAction("Login");
        }
    }
}
