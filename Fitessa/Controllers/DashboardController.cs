using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Fitessa.Data.Entities;
using Fitessa.Services.Interfaces;
using Fitessa.Models;
using AutoMapper;

namespace Fitessa.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWorkoutProgramService _workoutProgramService;
        private readonly IMealPlanService _mealPlanService;
        private readonly IExerciseService _exerciseService;
        private readonly IMeasurementLogService _measurementLogService;
        private readonly IProgressInsightsService _progressInsightsService;
        private readonly IFitnessAnalyticsService _fitnessAnalyticsService;
        private readonly IMapper _mapper;

        public DashboardController(
            UserManager<ApplicationUser> userManager,
            IWorkoutProgramService workoutProgramService,
            IMealPlanService mealPlanService,
            IExerciseService exerciseService,
            IMeasurementLogService measurementLogService,
            IProgressInsightsService progressInsightsService,
            IFitnessAnalyticsService fitnessAnalyticsService,
            IMapper mapper)
        {
            _userManager = userManager;
            _workoutProgramService = workoutProgramService;
            _mealPlanService = mealPlanService;
            _exerciseService = exerciseService;
            _measurementLogService = measurementLogService;
            _progressInsightsService = progressInsightsService;
            _fitnessAnalyticsService = fitnessAnalyticsService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var dashboardViewModel = new UserDashboardViewModel
            {
                User = user,
                RecentWorkouts = _workoutProgramService.GetAll().Take(3).ToList(),
                RecentMealPlans = _mealPlanService.GetByUser(user.Id).Take(3).ToList(),
                ProgressLogs = _measurementLogService.GetByUser(user.Id).Take(5).ToList(),
                ProgressInsights = _progressInsightsService.GetInsights(user.Id)
            };

            // Calculate fitness metrics using business logic
            var latestMeasurement = _measurementLogService.GetByUser(user.Id).OrderByDescending(m => m.LoggedAt).FirstOrDefault();
            if (latestMeasurement != null && user.Age > 0)
            {
                dashboardViewModel.BMI = _fitnessAnalyticsService.CalculateBMI((double)latestMeasurement.WeightKg, latestMeasurement.HeightCm);
                dashboardViewModel.BMICategory = _fitnessAnalyticsService.GetBMICategory(dashboardViewModel.BMI);
                dashboardViewModel.BMR = _fitnessAnalyticsService.CalculateBMR((double)latestMeasurement.WeightKg, latestMeasurement.HeightCm, user.Age, user.Gender ?? "Unknown");
                dashboardViewModel.TDEE = _fitnessAnalyticsService.CalculateTDEE(dashboardViewModel.BMR, "sedentary");
                
                if (user.GoalValue.HasValue)
                {
                    dashboardViewModel.WeightProgress = _fitnessAnalyticsService.CalculateProgressPercentage((double)latestMeasurement.WeightKg, (double)user.GoalValue.Value);
                    dashboardViewModel.WeightStatus = _fitnessAnalyticsService.GetProgressStatus(dashboardViewModel.WeightProgress);
                }
            }

            // Get personalized recommendations
            dashboardViewModel.WorkoutRecommendations = _fitnessAnalyticsService.GenerateWorkoutRecommendations(
                "Beginner", 
                user.Goal ?? "Maintenance", 
                (double)(latestMeasurement?.WeightKg ?? 70), 
                (double)(user.GoalValue ?? 70));

            dashboardViewModel.NutritionRecommendations = _fitnessAnalyticsService.GenerateNutritionRecommendations(
                user.Goal ?? "Maintenance", 
                (double)(latestMeasurement?.WeightKg ?? 70), 
                (double)(user.GoalValue ?? 70));

            return View(dashboardViewModel);
        }

        public async Task<IActionResult> Analytics()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var startDate = DateTime.Now.AddDays(-30);
            var endDate = DateTime.Now;

            var latestMeasurement = _measurementLogService.GetByUser(user.Id).OrderByDescending(m => m.LoggedAt).FirstOrDefault();
            
            var analyticsViewModel = new AnalyticsViewModel
            {
                PerformanceMetrics = _fitnessAnalyticsService.CalculatePerformanceMetrics(user.Id, startDate, endDate),
                ProgressTrends = _fitnessAnalyticsService.AnalyzeProgressTrends(user.Id, "weight", 30),
                ConsistencyScore = _fitnessAnalyticsService.CalculateConsistencyScore(user.Id, startDate, endDate),
                PersonalizedWorkouts = _fitnessAnalyticsService.GetPersonalizedWorkouts(user.Id, user.Goal ?? "Maintenance", "Beginner"),
                PersonalizedNutrition = _fitnessAnalyticsService.GetPersonalizedNutrition(user.Id, user.Goal ?? "Maintenance", (double)(latestMeasurement?.WeightKg ?? 70))
            };

            // Calculate body composition metrics
            if (latestMeasurement != null && user.Age > 0)
            {
                var bmi = _fitnessAnalyticsService.CalculateBMI((double)latestMeasurement.WeightKg, latestMeasurement.HeightCm);
                analyticsViewModel.BodyFatPercentage = _fitnessAnalyticsService.CalculateBodyFatPercentage(bmi, user.Age, user.Gender ?? "Unknown");
                analyticsViewModel.LeanBodyMass = _fitnessAnalyticsService.CalculateLeanBodyMass((double)latestMeasurement.WeightKg, analyticsViewModel.BodyFatPercentage);
            }

            return View(analyticsViewModel);
        }

        public async Task<IActionResult> Goals()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var latestMeasurement = _measurementLogService.GetByUser(user.Id).OrderByDescending(m => m.LoggedAt).FirstOrDefault();
            
            var goalsViewModel = new GoalsViewModel
            {
                User = user,
                CurrentWeight = (double)(latestMeasurement?.WeightKg ?? 70),
                TargetWeight = (double)(user.GoalValue ?? 70)
            };

            if (latestMeasurement != null && user.GoalValue.HasValue)
            {
                goalsViewModel.WeightProgress = _fitnessAnalyticsService.CalculateProgressPercentage((double)latestMeasurement.WeightKg, (double)user.GoalValue.Value);
                goalsViewModel.IsGoalAchieved = _fitnessAnalyticsService.IsGoalAchieved((double)latestMeasurement.WeightKg, (double)user.GoalValue.Value);
                goalsViewModel.Milestones = _fitnessAnalyticsService.GenerateMilestones((double)latestMeasurement.WeightKg, (double)user.GoalValue.Value, 5);
            }

            return View(goalsViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateGoals(GoalsViewModel model)
        {
            if (!ModelState.IsValid) return View("Goals", model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            user.GoalValue = (decimal)model.TargetWeight;
            user.Goal = model.FitnessGoal;

            await _userManager.UpdateAsync(user);

            TempData["Success"] = "Goals updated successfully!";
            return RedirectToAction(nameof(Goals));
        }

        public async Task<IActionResult> NutritionAnalysis(int mealPlanId)
        {
            var mealPlan = _mealPlanService.GetById(mealPlanId);
            if (mealPlan == null) return NotFound();

            var analysis = _fitnessAnalyticsService.AnalyzeMealPlan(mealPlan);

            var nutritionViewModel = new NutritionAnalysisViewModel
            {
                MealPlan = mealPlan,
                Analysis = analysis
            };

            return View(nutritionViewModel);
        }

        public async Task<IActionResult> WorkoutIntensity(int workoutId)
        {
            var workout = _workoutProgramService.GetById(workoutId);
            if (workout == null) return NotFound();

            var exercises = _exerciseService.GetAll().Take(5).ToList(); // Simulated exercises
            var intensity = _fitnessAnalyticsService.CalculateWorkoutIntensity(exercises, workout.DurationDays * 30);

            var intensityViewModel = new WorkoutIntensityViewModel
            {
                WorkoutProgram = workout,
                Exercises = exercises,
                Intensity = intensity,
                IntensityLevel = intensity switch
                {
                    < 1.5 => "Low",
                    < 2.5 => "Moderate",
                    < 3.5 => "High",
                    _ => "Very High"
                }
            };

            return View(intensityViewModel);
        }
    }
} 