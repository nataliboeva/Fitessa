using System.Diagnostics;
using Fitessa.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Fitessa.Data.Entities;
using Fitessa.Services.Interfaces;

namespace Fitessa.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWorkoutProgramService _workoutProgramService;
        private readonly IMealPlanService _mealPlanService;
        private readonly IMeasurementLogService _measurementLogService;
        private readonly IProgressInsightsService _progressInsightsService;

        public HomeController(
            ILogger<HomeController> logger,
            UserManager<ApplicationUser> userManager,
            IWorkoutProgramService workoutProgramService,
            IMealPlanService mealPlanService,
            IMeasurementLogService measurementLogService,
            IProgressInsightsService progressInsightsService)
        {
            _logger = logger;
            _userManager = userManager;
            _workoutProgramService = workoutProgramService;
            _mealPlanService = mealPlanService;
            _measurementLogService = measurementLogService;
            _progressInsightsService = progressInsightsService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                if (User.Identity?.IsAuthenticated == true)
                {
                    var user = await _userManager.GetUserAsync(User);
                    if (user != null)
                    {
                        ViewBag.User = user;
                        
                        try
                        {
                            ViewBag.RecommendedWorkouts = _workoutProgramService.GetRecommended(user.Goal);
                            ViewBag.RecommendedMeals = _mealPlanService.GetRecommended(user.Goal);
                            ViewBag.ProgressInsights = _progressInsightsService.GetInsights(user.Id);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, "Error loading recommendations for user {UserId}", user.Id);
                            // Continue without recommendations
                        }
                        
                        // Calculate progress and streak
                        var progressData = await CalculateProgressData(user);
                        ViewBag.ProgressData = progressData;
                    }
                }
                
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading home page");
                // Return view with error message
                ViewBag.ErrorMessage = "Unable to load dashboard. Please try again later.";
                return View();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task<object> CalculateProgressData(ApplicationUser user)
        {
            var logs = _measurementLogService.GetByUser(user.Id).OrderByDescending(l => l.LoggedAt).ToList();
            var latestWeight = logs.FirstOrDefault()?.WeightKg;
            var goalValue = user.GoalValue;
            var progressPercent = 0.0;
            var goalAchieved = false;
            var streak = 0;

            // Calculate progress percentage
            if (user.GoalType == "Weight" && latestWeight.HasValue && goalValue.HasValue)
            {
                if (goalValue < latestWeight)
                    progressPercent = (double)((latestWeight - goalValue) / (latestWeight - goalValue + 0.01m)) * 100;
                else
                    progressPercent = (double)((goalValue - latestWeight) / (goalValue - latestWeight + 0.01m)) * 100;
                
                if ((goalValue < latestWeight && latestWeight <= goalValue) || 
                    (goalValue > latestWeight && latestWeight >= goalValue))
                    goalAchieved = true;
            }

            // Calculate streak
            if (logs.Any())
            {
                var today = DateTime.UtcNow.Date;
                foreach (var log in logs)
                {
                    if ((today - log.LoggedAt.Date).TotalDays == streak)
                        streak++;
                    else
                        break;
                }
            }

            return new
            {
                LatestWeight = latestWeight,
                GoalValue = goalValue,
                ProgressPercent = progressPercent,
                GoalAchieved = goalAchieved,
                Streak = streak
            };
        }
    }
}
