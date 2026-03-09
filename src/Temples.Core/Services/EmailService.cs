using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Temples.Core.Interfaces;

namespace Temples.Core.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendPasswordResetEmailAsync(string toEmail, string resetLink)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(
            _configuration["Email:SenderName"] ?? "宮廟系統",
            _configuration["Email:SenderEmail"] ?? "noreply@temple.local"));
        message.To.Add(MailboxAddress.Parse(toEmail));
        message.Subject = "密碼重設 - 宮廟系統";

        message.Body = new TextPart("html")
        {
            Text = $"""
                <h2>密碼重設</h2>
                <p>您好，</p>
                <p>我們收到了您的密碼重設請求。請點擊以下連結重設密碼：</p>
                <p><a href="{resetLink}">重設密碼</a></p>
                <p>如果您沒有提出此請求，請忽略此信件。</p>
                <p>此連結將在 24 小時後失效。</p>
                """
        };

        using var client = new SmtpClient();
        await client.ConnectAsync(
            _configuration["Email:SmtpHost"] ?? "smtp.gmail.com",
            int.Parse(_configuration["Email:SmtpPort"] ?? "587"),
            MailKit.Security.SecureSocketOptions.StartTls);

        await client.AuthenticateAsync(
            _configuration["Email:SenderEmail"],
            _configuration["Email:Password"]);

        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}
