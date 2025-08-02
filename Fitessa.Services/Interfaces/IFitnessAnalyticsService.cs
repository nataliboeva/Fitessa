using Fitessa.Data.Entities;

namespace Fitessa.Services.Interfaces
{
    public interface IFitnessAnalyticsService
    {
        // BMI and Body Composition Calculations
        double CalculateBMI(double weightKg, double heightCm);
        string GetBMICategory(double bmi);
        double CalculateBodyFatPercentage(double bmi, int age, string gender);
        double CalculateLeanBodyMass(double weightKg, double bodyFatPercentage);
        
        // Calorie Calculations
        double CalculateBMR(double weightKg, double heightCm, int age, string gender);
        double CalculateTDEE(double bmr, string activityLevel);
        double CalculateCalorieDeficit(double tdee, double targetWeightLossKg);
        double CalculateCalorieSurplus(double tdee, double targetWeightGainKg);
        
        // Workout Intensity and Progress
        double CalculateWorkoutIntensity(List<Exercise> exercises, int durationMinutes);
        double CalculateProgressPercentage(double currentValue, double targetValue);
        string GetProgressStatus(double progressPercentage);
        List<string> GenerateWorkoutRecommendations(string fitnessLevel, string goal, double currentWeight, double targetWeight);
        
        // Goal Achievement Tracking
        bool IsGoalAchieved(double currentValue, double targetValue, double tolerance = 0.05);
        double CalculateGoalProgress(double currentValue, double targetValue);
        List<GoalMilestone> GenerateMilestones(double startValue, double targetValue, int numberOfMilestones);
        
        // Nutrition Analysis
        NutritionAnalysis AnalyzeMealPlan(MealPlan mealPlan);
        List<string> GenerateNutritionRecommendations(string goal, double currentWeight, double targetWeight);
        double CalculateMacroRatio(double protein, double carbs, double fat);
        
        // Performance Metrics
        PerformanceMetrics CalculatePerformanceMetrics(string userId, DateTime startDate, DateTime endDate);
        List<ProgressTrend> AnalyzeProgressTrends(string userId, string metricType, int days);
        double CalculateConsistencyScore(string userId, DateTime startDate, DateTime endDate);
        
        // Personalized Recommendations
        List<WorkoutRecommendation> GetPersonalizedWorkouts(string userId, string goal, string fitnessLevel);
        List<NutritionRecommendation> GetPersonalizedNutrition(string userId, string goal, double currentWeight);
        List<LifestyleRecommendation> GetLifestyleRecommendations(string userId, PerformanceMetrics metrics);
    }

    public class NutritionAnalysis
    {
        public double TotalCalories { get; set; }
        public double ProteinGrams { get; set; }
        public double CarbsGrams { get; set; }
        public double FatGrams { get; set; }
        public double ProteinPercentage { get; set; }
        public double CarbsPercentage { get; set; }
        public double FatPercentage { get; set; }
        public string MacroBalance { get; set; }
        public List<string> Recommendations { get; set; } = new();
    }

    public class PerformanceMetrics
    {
        public double AverageWorkoutDuration { get; set; }
        public double WorkoutFrequency { get; set; }
        public double ProgressRate { get; set; }
        public double ConsistencyScore { get; set; }
        public List<string> Strengths { get; set; } = new();
        public List<string> AreasForImprovement { get; set; } = new();
    }

    public class ProgressTrend
    {
        public DateTime Date { get; set; }
        public double Value { get; set; }
        public double Change { get; set; }
        public string Trend { get; set; }
    }

    public class GoalMilestone
    {
        public int MilestoneNumber { get; set; }
        public double TargetValue { get; set; }
        public DateTime EstimatedDate { get; set; }
        public bool IsAchieved { get; set; }
        public string Description { get; set; } = string.Empty;
    }

    public class WorkoutRecommendation
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Difficulty { get; set; } = string.Empty;
        public int DurationMinutes { get; set; }
        public List<string> Benefits { get; set; } = new();
        public double ConfidenceScore { get; set; }
    }

    public class NutritionRecommendation
    {
        public string MealType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Calories { get; set; }
        public double Protein { get; set; }
        public double Carbs { get; set; }
        public double Fat { get; set; }
        public List<string> Ingredients { get; set; } = new();
        public List<string> Benefits { get; set; } = new();
    }

    public class LifestyleRecommendation
    {
        public string Category { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Priority { get; set; }
        public List<string> ActionSteps { get; set; } = new();
    }
} 