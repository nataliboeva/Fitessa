using Fitessa.Data;
using Fitessa.Data.Entities;
using Fitessa.Services.Interfaces;

namespace Fitessa.Services.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext _context;

        public NotificationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Notification> GetAll()
        {
            return _context.Notifications.ToList();
        }

        public IEnumerable<Notification> GetByUser(string userId)
        {
            return _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.DeliveryTime)
                .ToList();
        }

        public Notification GetById(int id)
        {
            return _context.Notifications.Find(id);
        }

        public void Create(Notification notification)
        {
            notification.DeliveryTime = DateTime.UtcNow;
            _context.Notifications.Add(notification);
            _context.SaveChanges();
        }

        public void Update(Notification notification)
        {
            _context.Notifications.Update(notification);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var notification = _context.Notifications.Find(id);
            if (notification != null)
            {
                _context.Notifications.Remove(notification);
                _context.SaveChanges();
            }
        }

        public async Task SendPersonalNotification(string userId, string message, string type = "info")
        {
            var user = _context.Users.Find(userId);
            if (user == null) return;

            var notification = new Notification
            {
                UserId = userId,
                Message = message,
                Type = type,
                Title = "Notification",
                DeliveryTime = DateTime.UtcNow,
                User = user
            };

            Create(notification);
        }

        public async Task SendGlobalNotification(string message, string type = "info")
        {
            var users = _context.Users.ToList();
            var notifications = new List<Notification>();

            foreach (var user in users)
            {
                notifications.Add(new Notification
                {
                    UserId = user.Id,
                    Message = message,
                    Type = type,
                    Title = "Global Notification",
                    DeliveryTime = DateTime.UtcNow,
                    User = user
                });
            }

            _context.Notifications.AddRange(notifications);
            _context.SaveChanges();
        }

        public async Task SendWorkoutReminder(string userId, string workoutName)
        {
            var message = $"Time for your workout: {workoutName}! 💪";
            await SendPersonalNotification(userId, message, "reminder");
        }

        public async Task SendGoalAchievement(string userId, string goalType, string achievement)
        {
            var message = $"Congratulations! You've achieved your {goalType} goal: {achievement} 🎉";
            await SendPersonalNotification(userId, message, "achievement");
        }
    }
} 