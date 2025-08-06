using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Fitessa.Data.Entities;
using Fitessa.Services.Interfaces;
using Fitessa.Models;
using AutoMapper;

namespace Fitessa.Web.Controllers
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
                .OrderBy(p => p.LoggedAt)
                .Take(10)
                .ToList();

            var labels = progressLogs.Select(p => p.LoggedAt.ToString("MM/dd")).ToArray();
            var weights = progressLogs.Select(p => p.WeightKg).ToArray();

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


    }
} 