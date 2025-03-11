using System.Net.Mail;
using System.Net;

public class EmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void SendEmail(string toEmail, string subject, string body, string bodyFilePath, Dictionary<string, string> placeholders)
    {
        var smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential("vaghasiyarutika6@gmail.com", "vqee upmp uibj chun"),
            EnableSsl = true
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress("vaghasiyarutika6@gmail.com"),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        mailMessage.To.Add(toEmail);
        smtpClient.Send(mailMessage);
    }

}
