using Fitessa.Data.Entities;
using Fitessa.Services.Interfaces;

namespace Fitessa.Models
{
    public class UserDashboardViewModel
    {
        public ApplicationUser User { get; set; } = new();
        public List<WorkoutProgram> RecentWorkouts { get; set; } = new();
        public List<MealPlan> RecentMealPlans { get; set; } = new();
        public List<MeasurementLog> ProgressLogs { get; set; } = new();
        public object ProgressInsights { get; set; } = new();
        
        // Fitness Metrics
        public double BMI { get; set; }
        public string BMICategory { get; set; } = string.Empty;
        public double BMR { get; set; }
        public double TDEE { get; set; }
        public double WeightProgress { get; set; }
        public string WeightStatus { get; set; } = string.Empty;
        
        // Recommendations
        public List<string> WorkoutRecommendations { get; set; } = new();
        public List<string> NutritionRecommendations { get; set; } = new();
    }

    public class AnalyticsViewModel
    {
        public PerformanceMetrics PerformanceMetrics { get; set; } = new();
        public List<ProgressTrend> ProgressTrends { get; set; } = new();
        public double ConsistencyScore { get; set; }
        public List<WorkoutRecommendation> PersonalizedWorkouts { get; set; } = new();
        public List<NutritionRecommendation> PersonalizedNutrition { get; set; } = new();
        
        // Body Composition
        public double BodyFatPercentage { get; set; }
        public double LeanBodyMass { get; set; }
    }

    public class GoalsViewModel
    {
        public ApplicationUser User { get; set; } = new();
        public double CurrentWeight { get; set; }
        public double TargetWeight { get; set; }
        public string FitnessGoal { get; set; } = string.Empty;
        public string ActivityLevel { get; set; } = string.Empty;
        
        // Progress Tracking
        public double WeightProgress { get; set; }
        public bool IsGoalAchieved { get; set; }
        public List<GoalMilestone> Milestones { get; set; } = new();
    }

    public class NutritionAnalysisViewModel
    {
        public MealPlan MealPlan { get; set; } = new();
        public NutritionAnalysis Analysis { get; set; } = new();
    }

    public class WorkoutIntensityViewModel
    {
        public WorkoutProgram WorkoutProgram { get; set; } = new();
        public List<Exercise> Exercises { get; set; } = new();
        public double Intensity { get; set; }
        public string IntensityLevel { get; set; } = string.Empty;
    }
} 