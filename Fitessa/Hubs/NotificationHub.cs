using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Fitessa.Services.Interfaces;

namespace Fitessa.Hubs
{
    public class NotificationHub : Hub, INotificationHub
    {
        public async Task SendNotification(string message, string type = "info")
        {
            await Clients.All.SendAsync("ReceiveNotification", message, type);
        }

        public async Task SendPersonalNotification(string userId, string message, string type = "info")
        {
            await Clients.User(userId).SendAsync("ReceiveNotification", message, type);
        }

        public async Task JoinUserGroup(string userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
        }

        public async Task LeaveUserGroup(string userId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
        }

        public async Task SendGroupNotification(string groupName, string message, string type = "info")
        {
            await Clients.Group(groupName).SendAsync("ReceiveNotification", message, type);
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
} 