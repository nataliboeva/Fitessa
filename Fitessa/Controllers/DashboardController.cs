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

            var dashboardViewModel = new DashboardViewModel
            {
                User = user,
                RecentWorkoutPrograms = _workoutProgramService.GetAll().Take(3).ToList(),
                RecentMealPlans = _mealPlanService.GetByUser(user.Id).Take(3).ToList(),
                RecentProgressLogs = _measurementLogService.GetByUser(user.Id).Take(5).ToList(),
                ProgressInsights = _progressInsightsService.GetInsights(user.Id)
            };

            return View(dashboardViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetProgressData()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Json(new { success = false });

            var progressLogs = _measurementLogService.GetByUser(user.Id)
                .OrderBy(p => p.Date)
                .Take(10)
                .ToList();

            var labels = progressLogs.Select(p => p.Date.ToString("MM/dd")).ToArray();
            var weights = progressLogs.Select(p => p.Weight).ToArray();

            return Json(new
            {
                success = true,
                labels = labels,
                weights = weights
            });
        }

        public async Task<IActionResult> Progress()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var progressViewModel = new ProgressViewModel
            {
                User = user,
                ProgressLogs = _measurementLogService.GetByUser(user.Id).ToList(),
                ProgressInsights = _progressInsightsService.GetInsights(user.Id)
            };

            return View(progressViewModel);
        }

        public async Task<IActionResult> Analytics()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var analyticsViewModel = new AnalyticsViewModel
            {
                User = user,
                ProgressLogs = _measurementLogService.GetByUser(user.Id).ToList(),
                WorkoutPrograms = _workoutProgramService.GetAll().ToList(),
                MealPlans = _mealPlanService.GetByUser(user.Id).ToList()
            };

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