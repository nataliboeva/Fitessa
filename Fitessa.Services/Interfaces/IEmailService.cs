namespace Fitessa.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
        Task SendWelcomeEmailAsync(string to, string userName);
        Task SendPasswordResetEmailAsync(string to, string resetLink);
        Task SendWorkoutReminderAsync(string to, string userName, string workoutName);
    }
} 