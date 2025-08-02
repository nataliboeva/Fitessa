using Fitessa.Data.Entities;

namespace Fitessa.Models
{
    public class AdminDashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalWorkoutPrograms { get; set; }
        public int TotalExercises { get; set; }
        public int TotalMealPlans { get; set; }
        public List<ApplicationUser> RecentUsers { get; set; } = new();
        public SystemStatsViewModel SystemStats { get; set; } = new();
    }

    public class SystemStatsViewModel
    {
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int TotalWorkouts { get; set; }
        public int TotalExercises { get; set; }
        public int TotalMealPlans { get; set; }
        public int TotalProgressLogs { get; set; }
    }

    public class AdminUserViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsBanned { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Role { get; set; } = string.Empty;
    }

    public class AdminUserDetailsViewModel
    {
        public ApplicationUser User { get; set; } = new();
        public List<WorkoutProgram> WorkoutPrograms { get; set; } = new();
        public List<MealPlan> MealPlans { get; set; } = new();
        public List<MeasurementLog> ProgressLogs { get; set; } = new();
        public object ProgressInsights { get; set; } = new();
    }
} 