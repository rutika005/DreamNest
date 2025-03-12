using System.IO;  // For reading HTML files
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

    // ✅ Load Email Template and Replace Dynamic Content
    private string LoadEmailTemplate(string templateName, Dictionary<string, string> replacements)
    {
        string path = Path.Combine(Directory.GetCurrentDirectory(), "Views", "EmailTemplates", templateName);
        string emailBody = File.ReadAllText(path);

        foreach (var item in replacements)
        {
            emailBody = emailBody.Replace(item.Key, item.Value);
        }
        return emailBody;
    }

    // ✅ Send Email with Template
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
        var user = _context.registeruser.FirstOrDefault(u => u.Id == userId);
        return user?.Email ?? "";  
    }
}
