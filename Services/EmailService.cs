using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using StajYerApp_API.Models;

/* 
 * app.mailersend.com
 * kullanıcı hiwagig108@inpsur.com
 * şifre Asdasdasd1 
 */
namespace StajYerApp_API.Services
{

    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
        Task SendVerificationCodeAsync(string toEmail, string code);
    }

    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtpSettings;

        public EmailService(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_smtpSettings.SenderName, _smtpSettings.SenderEmail));
            message.To.Add(new MailboxAddress(toEmail, toEmail));
            message.Subject = subject;
            message.Body = new TextPart("html") { Text = body };

            using var client = new SmtpClient();
            await client.ConnectAsync(_smtpSettings.Server, _smtpSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

        public async Task SendVerificationCodeAsync(string toEmail, string code)
        {
            string subject = "Your Verification Code";
            string body = $@"
<!DOCTYPE html>
<html>

<head>
    <style>
        .container {{
            font-family: Arial, sans-serif;
            margin: 0 auto;
            padding: 20px;
            max-width: 600px;
            background-color: #f9f9f9;
            border: 1px solid #ddd;
            border-radius: 5px;
        }}

        .header {{
            text-align: center;
            padding: 10px;
            background-color: #4CAF50;
            color: white;
            border-radius: 5px 5px 0 0;
        }}

        .content {{
            padding: 20px;
        }}

        .code {{
            font-size: 24px;
            font-weight: bold;
            color: #333;
        }}

        .footer {{
            text-align: center;
            margin-top: 20px;
            font-size: 12px;
            color: #777;
        }}
        #footerimg{{
            width: 85px;
            height: auto;
            display: flex;
        }}
    </style>
</head>

<body>
    <div class='container'>
        <div class='header'>
            <h2>Your Verification Code</h2>
        </div>
        <div class='content'>
            <p>Dear User,</p>
            <p>Your verification code is:</p>
            <p class='code'>{code}</p>
            <p>Please use this code to complete your password reset request.</p>
            <p>If you did not request a password reset, please ignore this email.</p>
        </div>
        <div class='footer'>
            <p>&copy; {DateTime.UtcNow.Year} StajYer. All rights reserved.</p>
            <img id='footerimg' src='../Public/images/stajyerlogoasıl.png' alt=''>
        </div>
    </div>
</body>

</html>";
            await SendEmailAsync(toEmail, subject, body);
        }
    }


}