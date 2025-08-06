using Fitessa.Data.Entities;

namespace Fitessa.Models
{
    public class AdminDashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalWorkoutPrograms { get; set; }
        public int TotalExercises { get; set; }
        public int TotalMealPlans { get; set; }
        public List<ApplicationUser> RecentUsers { get; set; }
        public SystemStatsViewModel SystemStats { get; set; }
    }

    public class AdminUserViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public bool IsBanned { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class AdminUserDetailsViewModel
    {
        public ApplicationUser User { get; set; }
        public List<WorkoutProgram> WorkoutPrograms { get; set; }
        public List<MealPlan> MealPlans { get; set; }
        public List<MeasurementLog> ProgressLogs { get; set; }
        public object ProgressInsights { get; set; }
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
} 