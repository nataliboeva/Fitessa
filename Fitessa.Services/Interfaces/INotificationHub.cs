using Microsoft.AspNetCore.SignalR;

namespace Fitessa.Services.Interfaces
{
    public interface INotificationHub
    {
        Task SendNotification(string message, string type = "info");
        Task SendPersonalNotification(string userId, string message, string type = "info");
        Task SendGroupNotification(string groupName, string message, string type = "info");
    }
} 