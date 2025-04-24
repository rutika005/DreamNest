using System.IO;  
using System.Net.Mail;
using System.Net;
using Aesthetica.Models;
using Microsoft.Extensions.Configuration;

public class EmailService
{
    private readonly IConfiguration _configuration;
    private readonly AppDbContext _context;

    public EmailService(IConfiguration configuration, AppDbContext context)
    {
        _configuration = configuration;
        _context = context;
    }

    private string LoadEmailTemplate(string templateName, Dictionary<string, string> replacements)
    {
        try
        {
            templateName = "EmailTemplate" + ".html";
            if (templateName.Contains("<") || templateName.Contains(">"))
            {
                throw new ArgumentException("Invalid template name provided.");
            }

            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "templates", templateName);

            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Email template not found: {path}");
            }

            string emailBody = File.ReadAllText(path);

            foreach (var replacement in replacements)
            {
                emailBody = emailBody.Replace(replacement.Key, replacement.Value);
            }

            return emailBody;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error loading email template: {ex.Message}");
        }
    }


    public void SendEmail(int userId, string subject, string templateName, Dictionary<string, string> replacements)
    {
        string toEmail = GetUserEmail(userId); 
        if (string.IsNullOrEmpty(toEmail)) return;

        string body = LoadEmailTemplate(templateName, replacements); // Load HTML Template

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
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        mailMessage.To.Add(toEmail);
        smtpClient.Send(mailMessage);
    }

    private string GetUserEmail(int userId)
    {
        var user = _context.userregister.FirstOrDefault(u => u.Id == userId);
        return user?.Email ?? "";  
    }

}
