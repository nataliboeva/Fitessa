using Fitessa.Services.Interfaces;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Fitessa.Services.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                var smtpSettings = _configuration.GetSection("SmtpSettings");
                var smtpServer = smtpSettings["Server"] ?? "smtp.gmail.com";
                var smtpPort = int.Parse(smtpSettings["Port"] ?? "587");
                var smtpUsername = smtpSettings["Username"] ?? "";
                var smtpPassword = smtpSettings["Password"] ?? "";
                var fromEmail = smtpSettings["FromEmail"] ?? "noreply@fitessa.com";

                using var client = new SmtpClient(smtpServer, smtpPort)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(smtpUsername, smtpPassword)
                };

                var message = new MailMessage
                {
                    From = new MailAddress(fromEmail, "Fitessa"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                message.To.Add(to);

                await client.SendMailAsync(message);
                _logger.LogInformation($"Email sent successfully to {to}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send email to {to}");
                throw;
            }
        }

        public async Task SendWelcomeEmailAsync(string to, string userName)
        {
            var subject = "Welcome to Fitessa!";
            var body = $@"
                <h2>Welcome to Fitessa, {userName}!</h2>
                <p>Thank you for joining our fitness community. We're excited to help you achieve your fitness goals.</p>
                <p>Get started by:</p>
                <ul>
                    <li>Creating your first workout plan</li>
                    <li>Setting up your meal preferences</li>
                    <li>Tracking your progress</li>
                </ul>
                <p>Best regards,<br>The Fitessa Team</p>";

            await SendEmailAsync(to, subject, body);
        }

        public async Task SendPasswordResetEmailAsync(string to, string resetLink)
        {
            var subject = "Password Reset Request";
            var body = $@"
                <h2>Password Reset Request</h2>
                <p>You have requested to reset your password. Click the link below to proceed:</p>
                <p><a href='{resetLink}'>Reset Password</a></p>
                <p>If you didn't request this, please ignore this email.</p>
                <p>Best regards,<br>The Fitessa Team</p>";

            await SendEmailAsync(to, subject, body);
        }

        public async Task SendWorkoutReminderAsync(string to, string userName, string workoutName)
        {
            var subject = "Workout Reminder";
            var body = $@"
                <h2>Time for your workout, {userName}!</h2>
                <p>Don't forget your scheduled workout: <strong>{workoutName}</strong></p>
                <p>Stay consistent and you'll see amazing results!</p>
                <p>Best regards,<br>The Fitessa Team</p>";

            await SendEmailAsync(to, subject, body);
        }
    }
} 