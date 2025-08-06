using Fitessa.Data.Entities;

namespace Fitessa.Services.Interfaces
{
    public interface INotificationService
    {
        IEnumerable<Notification> GetAll();
        IEnumerable<Notification> GetByUser(string userId);
        Notification GetById(int id);
        void Create(Notification notification);
        void Update(Notification notification);
        void Delete(int id);
        Task SendPersonalNotification(string userId, string message, string type = "info");
        Task SendGlobalNotification(string message, string type = "info");
        Task SendWorkoutReminder(string userId, string workoutName);
        Task SendGoalAchievement(string userId, string goalType, string achievement);
    }
} 