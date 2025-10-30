using AuthService.API.Interfaces;
using MailKit.Net.Smtp;
using MimeKit;

namespace AuthService.API.Services;

public class EmailService(IConfiguration config) : IEmailService
{
    public async Task SendEmailAsync(string toEmail, string subject, string htmlBody)
    {
        var emailSection = config.GetSection("Email");
        var user = emailSection["User"];
        var pass = emailSection["Password"];
        var host = emailSection["SmtpHost"] ?? "smtp.gmail.com";
        var port = int.Parse(emailSection["SmtpPort"] ?? "587");
        var fromName = emailSection["FromName"] ?? "App";
        var fromEmail = emailSection["FromEmail"] ?? user;

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(fromName, fromEmail));
        message.To.Add(MailboxAddress.Parse(toEmail));
        message.Subject = subject;

        var bodyBuilder = new BodyBuilder { HtmlBody = htmlBody };
        message.Body = bodyBuilder.ToMessageBody();

        using var client = new SmtpClient();
        await client.ConnectAsync(host, port, MailKit.Security.SecureSocketOptions.StartTls);

        await client.AuthenticateAsync(user, pass);

        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}