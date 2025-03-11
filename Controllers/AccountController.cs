using Aesthetica.Models;
using Microsoft.AspNetCore.Mvc;

public class AccountController : Controller
{
    private readonly EmailService _emailService;

    public AccountController(EmailService emailService)
    {
        _emailService = emailService;
    }

    [HttpPost]
    public IActionResult Register(RegisterModel model)
    {
        if (ModelState.IsValid)
        {
            // Define email template file path
            string bodyFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/templates/EmailTemplate.html");

            // Create placeholders for dynamic values in the email template
            var placeholders = new Dictionary<string, string>
        {
            { "{FullName}", model.Name }
        };

            // Send the welcome email
            _emailService.SendEmail("vaghasiyarutika6@gmail.com", model.Email, "Welcome to Aesthetica!", bodyFilePath, placeholders);

            return RedirectToAction("Index", "Home");  // Redirect to home page after successful registration
        }

        return View(model);
    }

}
