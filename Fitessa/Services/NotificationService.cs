using Microsoft.AspNetCore.SignalR;
using Fitessa.Hubs;

namespace Fitessa.Services
{
    public class NotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendGlobalNotification(string message, string type = "info")
        {
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", message, type);
        }

        public async Task SendPersonalNotification(string userId, string message, string type = "info")
        {
            await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", message, type);
        }

        public async Task SendGroupNotification(string groupName, string message, string type = "info")
        {
            await _hubContext.Clients.Group(groupName).SendAsync("ReceiveNotification", message, type);
        }

        public async Task SendWorkoutReminder(string userId, string workoutName)
        {
            var message = $"Time for your workout: {workoutName}! 💪";
            await SendPersonalNotification(userId, message, "info");
        }

        public async Task SendGoalAchievement(string userId, string goalType, string achievement)
        {
            var message = $"🎉 Congratulations! You've achieved your {goalType} goal: {achievement}";
            await SendPersonalNotification(userId, message, "success");
        }

        public async Task SendProgressUpdate(string userId, string progressType, string value)
        {
            var message = $"📊 Your {progressType} has been updated: {value}";
            await SendPersonalNotification(userId, message, "info");
        }

        public async Task SendPaymentConfirmation(string userId, string planName, decimal amount)
        {
            var message = $"✅ Payment successful! You've subscribed to {planName} for ${amount}";
            await SendPersonalNotification(userId, message, "success");
        }

        public async Task SendSystemMaintenance(string message)
        {
            await SendGlobalNotification($"🔧 System Maintenance: {message}", "warning");
        }

        public async Task SendNewFeature(string featureName, string description)
        {
            var message = $"🆕 New Feature: {featureName} - {description}";
            await SendGlobalNotification(message, "info");
        }
    }
} 