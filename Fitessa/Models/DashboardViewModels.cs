using Fitessa.Data.Entities;
using Fitessa.Services.Interfaces;

namespace Fitessa.Models
{
    public class DashboardViewModel
    {
        public ApplicationUser User { get; set; }
        public List<WorkoutProgram> RecentWorkoutPrograms { get; set; }
        public List<MealPlan> RecentMealPlans { get; set; }
        public List<MeasurementLog> RecentProgressLogs { get; set; }
        public object ProgressInsights { get; set; }
    }

    public class ProgressViewModel
    {
        public ApplicationUser User { get; set; }
        public List<MeasurementLog> ProgressLogs { get; set; }
        public object ProgressInsights { get; set; }
    }

    public class AnalyticsViewModel
    {
        public ApplicationUser User { get; set; }
        public List<MeasurementLog> ProgressLogs { get; set; }
        public List<WorkoutProgram> WorkoutPrograms { get; set; }
        public List<MealPlan> MealPlans { get; set; }
    }

    public class NotificationViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
} 