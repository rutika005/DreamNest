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
        private readonly AppDbContext _context;  
        private readonly EmailService _emailService;  
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
                _context.contact.Add(model);  
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
            
                model.token = Guid.NewGuid().ToString(); 
                model.IsVerified = false; 
                Console.WriteLine("Generated Token: " + model.token);

                _context.userregister.Add(model);
                _context.SaveChanges();

                string verificationLink = Url.Action("VerifyEmail", "Home", new { model.token }, Request.Scheme);

                string emailBody = $"<h2>Welcome, {model.Name}!</h2><p>Please verify your email by clicking <a href='{verificationLink}'>here</a>.</p>";

                var replacements = new Dictionary<string, string>
                {
                    { "{Name}", model.Name },
                    { "{VERIFY_LINK}", $"https://localhost:7150/Home/VerifyEmail?token={model.token}" }
                };

                _emailService.SendEmail(model.Id, "Verify Your Email", emailBody, replacements);
                Console.WriteLine($"Name: {model.Name}, Email: {model.Email}, Token: {model.token}");


              
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

            var admin = _context.admin.FirstOrDefault(a => a.Email == model.Email);

            if (admin != null)
            {
                Console.WriteLine($"Admin Password Type: {admin.Pass}");

                string adminPassword = admin.Pass.Trim();

                if (admin != null && !string.IsNullOrEmpty(admin.Pass))
                {
                    Console.WriteLine("Admin login successful!");
                    HttpContext.Session.SetString("UserEmail", admin.Email);
                    HttpContext.Session.SetString("UserRole", "Admin");
                    Console.WriteLine($"Pass:  { model.Pass }");

                    return RedirectToAction("Index", "Admin"); 
                }
                else
                {
                    Console.WriteLine("Incorrect admin password!");
                    ViewBag.Message = "Incorrect password!";
                    return View(model);
                }
            }

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

            var newToken = Guid.NewGuid().ToString();

            user.token = newToken;
            user.ResetTokenExpiry = DateTime.UtcNow.AddHours(1);

            _context.SaveChanges();

            string resetLink = Url.Action("Resetpass", "Home", new { token = newToken }, Request.Scheme);

            var replacements = new string[] { "{VERIFY_LINK}", $"https://localhost:7150/Home/Resetpass?token={user.token = newToken}" };

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

            _context.Entry(user).Property(u => u.Pass).IsModified = true;
            _context.Entry(user).Property(u => u.token).IsModified = true;
            user.ResetTokenExpiry = null;

            await _context.SaveChangesAsync();

            ViewBag.Message = "Password successfully reset! You can now login.";
            return RedirectToAction("Login");
        }
    }
}
