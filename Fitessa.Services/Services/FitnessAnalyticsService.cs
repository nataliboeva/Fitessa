using Fitessa.Services.Interfaces;
using Fitessa.Data.Entities;
using Fitessa.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Fitessa.Services.Services
{
    public class FitnessAnalyticsService : IFitnessAnalyticsService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<FitnessAnalyticsService> _logger;

        public FitnessAnalyticsService(ApplicationDbContext context, ILogger<FitnessAnalyticsService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public double CalculateBMI(double weightKg, double heightCm)
        {
            if (heightCm <= 0 || weightKg <= 0) return 0;
            
            double heightM = heightCm / 100;
            return Math.Round(weightKg / (heightM * heightM), 1);
        }

        public string GetBMICategory(double bmi)
        {
            return bmi switch
            {
                < 18.5 => "Underweight",
                < 25 => "Normal weight",
                < 30 => "Overweight",
                < 35 => "Obese (Class I)",
                < 40 => "Obese (Class II)",
                _ => "Obese (Class III)"
            };
        }

        public double CalculateBodyFatPercentage(double bmi, int age, string gender)
        {
            if (age <= 0) return 0;

            double baseBodyFat = gender.ToLower() switch
            {
                "male" => 15.0 + (bmi - 22) * 0.5,
                "female" => 25.0 + (bmi - 22) * 0.5,
                _ => 20.0 + (bmi - 22) * 0.5
            };

            double ageAdjustment = age switch
            {
                < 30 => 0,
                < 40 => 1,
                < 50 => 2,
                < 60 => 3,
                _ => 4
            };

            return Math.Max(5, Math.Min(50, Math.Round(baseBodyFat + ageAdjustment, 1)));
        }

        public double CalculateLeanBodyMass(double weightKg, double bodyFatPercentage)
        {
            return Math.Round(weightKg * (1 - bodyFatPercentage / 100), 1);
        }

        public double CalculateBMR(double weightKg, double heightCm, int age, string gender)
        {
            return gender.ToLower() switch
            {
                "male" => 88.362 + (13.397 * weightKg) + (4.799 * heightCm) - (5.677 * age),
                "female" => 447.593 + (9.247 * weightKg) + (3.098 * heightCm) - (4.330 * age),
                _ => 88.362 + (13.397 * weightKg) + (4.799 * heightCm) - (5.677 * age)
            };
        }

        public double CalculateTDEE(double bmr, string activityLevel)
        {
            double multiplier = activityLevel.ToLower() switch
            {
                "sedentary" => 1.2,
                "lightly active" => 1.375,
                "moderately active" => 1.55,
                "very active" => 1.725,
                "extremely active" => 1.9,
                _ => 1.2
            };

            return Math.Round(bmr * multiplier);
        }

        public double CalculateCalorieDeficit(double tdee, double targetWeightLossKg)
        {
            double weeklyDeficit = targetWeightLossKg * 7700; // 7700 calories per kg
            double dailyDeficit = weeklyDeficit / 7;
            return Math.Max(500, Math.Min(1000, dailyDeficit));
        }

        public double CalculateCalorieSurplus(double tdee, double targetWeightGainKg)
        {
            double weeklySurplus = targetWeightGainKg * 7700;
            double dailySurplus = weeklySurplus / 7;
            return Math.Max(200, Math.Min(500, dailySurplus));
        }

        public double CalculateWorkoutIntensity(List<Exercise> exercises, int durationMinutes)
        {
            if (exercises == null || !exercises.Any() || durationMinutes <= 0) return 0;

            double totalIntensity = exercises.Sum(e => 
            {
                return e.DifficultyLevel?.ToLower() switch
                {
                    "beginner" => 1.0,
                    "intermediate" => 2.0,
                    "advanced" => 3.0,
                    _ => 1.5
                };
            });

            double averageIntensity = totalIntensity / exercises.Count;
            double timeFactor = Math.Min(2.0, durationMinutes / 30.0);
            
            return Math.Round(averageIntensity * timeFactor, 2);
        }

        public double CalculateProgressPercentage(double currentValue, double targetValue)
        {
            if (targetValue == 0) return 0;
            return Math.Round((currentValue / targetValue) * 100, 1);
        }

        public string GetProgressStatus(double progressPercentage)
        {
            return progressPercentage switch
            {
                < 25 => "Needs Improvement",
                < 50 => "Making Progress",
                < 75 => "Good Progress",
                < 100 => "Almost There",
                100 => "Goal Achieved!",
                _ => "Exceeded Goal!"
            };
        }

        public List<string> GenerateWorkoutRecommendations(string fitnessLevel, string goal, double currentWeight, double targetWeight)
        {
            var recommendations = new List<string>();
            double weightDifference = targetWeight - currentWeight;

            if (goal.ToLower() == "weight loss" && weightDifference < 0)
            {
                recommendations.Add("Focus on cardio exercises 3-4 times per week");
                recommendations.Add("Include strength training 2-3 times per week to preserve muscle");
                recommendations.Add("Aim for 150-300 minutes of moderate cardio weekly");
            }
            else if (goal.ToLower() == "muscle gain" && weightDifference > 0)
            {
                recommendations.Add("Prioritize strength training 4-5 times per week");
                recommendations.Add("Include compound exercises like squats, deadlifts, and bench press");
                recommendations.Add("Limit cardio to 2-3 sessions per week");
            }
            else if (goal.ToLower() == "maintenance")
            {
                recommendations.Add("Balance cardio and strength training 3-4 times per week");
                recommendations.Add("Include flexibility and mobility work");
                recommendations.Add("Vary your workout intensity throughout the week");
            }

            if (fitnessLevel.ToLower() == "beginner")
            {
                recommendations.Add("Start with bodyweight exercises and gradually increase intensity");
                recommendations.Add("Focus on proper form and technique");
                recommendations.Add("Allow adequate rest between workouts");
            }

            return recommendations;
        }

        public bool IsGoalAchieved(double currentValue, double targetValue, double tolerance = 0.05)
        {
            double difference = Math.Abs(currentValue - targetValue);
            return difference <= (targetValue * tolerance);
        }

        public double CalculateGoalProgress(double currentValue, double targetValue)
        {
            if (targetValue == 0) return 0;
            return Math.Round((currentValue / targetValue) * 100, 1);
        }

        public List<GoalMilestone> GenerateMilestones(double startValue, double targetValue, int numberOfMilestones)
        {
            var milestones = new List<GoalMilestone>();
            double increment = (targetValue - startValue) / numberOfMilestones;

            for (int i = 1; i <= numberOfMilestones; i++)
            {
                double milestoneValue = startValue + (increment * i);
                milestones.Add(new GoalMilestone
                {
                    MilestoneNumber = i,
                    TargetValue = Math.Round(milestoneValue, 1),
                    EstimatedDate = DateTime.Now.AddDays(i * 7),
                    IsAchieved = false,
                    Description = $"Milestone {i}: {Math.Round(milestoneValue, 1)}"
                });
            }

            return milestones;
        }

        public NutritionAnalysis AnalyzeMealPlan(MealPlan mealPlan)
        {
            var analysis = new NutritionAnalysis();
            
            // Simulated nutrition data based on meal plan
            analysis.TotalCalories = 1800;
            analysis.ProteinGrams = 120;
            analysis.CarbsGrams = 200;
            analysis.FatGrams = 60;
            
            double totalMacros = analysis.ProteinGrams + analysis.CarbsGrams + analysis.FatGrams;
            analysis.ProteinPercentage = Math.Round((analysis.ProteinGrams / totalMacros) * 100, 1);
            analysis.CarbsPercentage = Math.Round((analysis.CarbsGrams / totalMacros) * 100, 1);
            analysis.FatPercentage = Math.Round((analysis.FatGrams / totalMacros) * 100, 1);
            
            analysis.MacroBalance = GetMacroBalanceDescription(analysis.ProteinPercentage, analysis.CarbsPercentage, analysis.FatPercentage);
            
            analysis.Recommendations.Add("Consider increasing protein intake for muscle building");
            analysis.Recommendations.Add("Ensure adequate fiber intake from whole grains and vegetables");
            analysis.Recommendations.Add("Stay hydrated with at least 8 glasses of water daily");
            
            return analysis;
        }

        public List<string> GenerateNutritionRecommendations(string goal, double currentWeight, double targetWeight)
        {
            var recommendations = new List<string>();
            double weightDifference = targetWeight - currentWeight;

            if (goal.ToLower() == "weight loss" && weightDifference < 0)
            {
                recommendations.Add("Create a moderate calorie deficit of 300-500 calories daily");
                recommendations.Add("Prioritize protein intake (1.6-2.2g per kg body weight)");
                recommendations.Add("Include plenty of fiber-rich foods for satiety");
                recommendations.Add("Limit added sugars and processed foods");
            }
            else if (goal.ToLower() == "muscle gain" && weightDifference > 0)
            {
                recommendations.Add("Consume 200-500 calories above maintenance");
                recommendations.Add("Aim for 1.6-2.2g protein per kg body weight");
                recommendations.Add("Include complex carbohydrates for energy");
                recommendations.Add("Time protein intake around workouts");
            }

            return recommendations;
        }

        public double CalculateMacroRatio(double protein, double carbs, double fat)
        {
            double total = protein + carbs + fat;
            if (total == 0) return 0;
            
            return Math.Round((protein / total) * 100, 1);
        }

        public PerformanceMetrics CalculatePerformanceMetrics(string userId, DateTime startDate, DateTime endDate)
        {
            var metrics = new PerformanceMetrics();
            
            // Simulated performance data
            metrics.AverageWorkoutDuration = 45.5;
            metrics.WorkoutFrequency = 3.2; // workouts per week
            metrics.ProgressRate = 0.8; // 80% progress rate
            metrics.ConsistencyScore = 85.0; // 85% consistency
            
            metrics.Strengths.Add("Consistent workout attendance");
            metrics.Strengths.Add("Good exercise form");
            metrics.Strengths.Add("Progressive overload implementation");
            
            metrics.AreasForImprovement.Add("Increase workout frequency to 4-5 times per week");
            metrics.AreasForImprovement.Add("Add more variety to exercises");
            metrics.AreasForImprovement.Add("Focus on recovery and rest days");
            
            return metrics;
        }

        public List<ProgressTrend> AnalyzeProgressTrends(string userId, string metricType, int days)
        {
            var trends = new List<ProgressTrend>();
            var random = new Random();
            
            for (int i = 0; i < days; i++)
            {
                double baseValue = metricType.ToLower() switch
                {
                    "weight" => 70.0,
                    "strength" => 100.0,
                    "endurance" => 30.0,
                    _ => 50.0
                };
                
                double change = random.Next(-2, 3);
                trends.Add(new ProgressTrend
                {
                    Date = DateTime.Now.AddDays(-i),
                    Value = Math.Round(baseValue + change, 1),
                    Change = change,
                    Trend = change > 0 ? "Increasing" : change < 0 ? "Decreasing" : "Stable"
                });
            }
            
            return trends.OrderBy(t => t.Date).ToList();
        }

        public double CalculateConsistencyScore(string userId, DateTime startDate, DateTime endDate)
        {
            // Simulated consistency calculation
            int totalDays = (endDate - startDate).Days;
            int workoutDays = (int)(totalDays * 0.75); // 75% consistency
            
            return Math.Round((double)workoutDays / totalDays * 100, 1);
        }

        public List<WorkoutRecommendation> GetPersonalizedWorkouts(string userId, string goal, string fitnessLevel)
        {
            var recommendations = new List<WorkoutRecommendation>();
            
            if (goal.ToLower() == "weight loss")
            {
                recommendations.Add(new WorkoutRecommendation
                {
                    Name = "High-Intensity Cardio Circuit",
                    Description = "30-minute circuit training combining cardio and strength",
                    Difficulty = "Intermediate",
                    DurationMinutes = 30,
                    Benefits = new List<string> { "Burns calories", "Improves cardiovascular health", "Builds endurance" },
                    ConfidenceScore = 0.85
                });
            }
            else if (goal.ToLower() == "muscle gain")
            {
                recommendations.Add(new WorkoutRecommendation
                {
                    Name = "Progressive Strength Training",
                    Description = "Focus on compound movements with progressive overload",
                    Difficulty = "Advanced",
                    DurationMinutes = 60,
                    Benefits = new List<string> { "Builds muscle mass", "Increases strength", "Improves bone density" },
                    ConfidenceScore = 0.90
                });
            }
            
            return recommendations;
        }

        public List<NutritionRecommendation> GetPersonalizedNutrition(string userId, string goal, double currentWeight)
        {
            var recommendations = new List<NutritionRecommendation>();
            
            if (goal.ToLower() == "weight loss")
            {
                recommendations.Add(new NutritionRecommendation
                {
                    MealType = "Breakfast",
                    Description = "High-protein breakfast with complex carbs",
                    Calories = 400,
                    Protein = 25,
                    Carbs = 45,
                    Fat = 15,
                    Ingredients = new List<string> { "Oatmeal", "Greek yogurt", "Berries", "Nuts" },
                    Benefits = new List<string> { "Sustains energy", "Supports muscle", "Controls appetite" }
                });
            }
            
            return recommendations;
        }

        public List<LifestyleRecommendation> GetLifestyleRecommendations(string userId, PerformanceMetrics metrics)
        {
            var recommendations = new List<LifestyleRecommendation>();
            
            if (metrics.ConsistencyScore < 80)
            {
                recommendations.Add(new LifestyleRecommendation
                {
                    Category = "Consistency",
                    Title = "Improve Workout Consistency",
                    Description = "Focus on building a regular workout routine",
                    Priority = 1,
                    ActionSteps = new List<string> 
                    { 
                        "Schedule workouts at the same time daily",
                        "Start with shorter, more manageable sessions",
                        "Track your progress to stay motivated"
                    }
                });
            }
            
            return recommendations;
        }

        private string GetMacroBalanceDescription(double protein, double carbs, double fat)
        {
            if (protein >= 25 && protein <= 35 && carbs >= 40 && carbs <= 60 && fat >= 20 && fat <= 35)
                return "Balanced";
            else if (protein > 35)
                return "High Protein";
            else if (carbs > 60)
                return "High Carb";
            else if (fat > 35)
                return "High Fat";
            else
                return "Needs Adjustment";
        }
    }
} 