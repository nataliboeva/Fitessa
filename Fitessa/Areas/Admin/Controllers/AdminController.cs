using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Fitessa.Data.Entities;
using Fitessa.Services.Interfaces;
using Fitessa.Models;
using AutoMapper;

namespace Fitessa.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWorkoutProgramService _workoutProgramService;
        private readonly IMealPlanService _mealPlanService;
        private readonly IExerciseService _exerciseService;
        private readonly IMeasurementLogService _measurementLogService;
        private readonly IProgressInsightsService _progressInsightsService;
        private readonly IMapper _mapper;

        public AdminController(
            UserManager<ApplicationUser> userManager,
            IWorkoutProgramService workoutProgramService,
            IMealPlanService mealPlanService,
            IExerciseService exerciseService,
            IMeasurementLogService measurementLogService,
            IProgressInsightsService progressInsightsService,
            IMapper mapper)
        {
            _userManager = userManager;
            _workoutProgramService = workoutProgramService;
            _mealPlanService = mealPlanService;
            _exerciseService = exerciseService;
            _measurementLogService = measurementLogService;
            _progressInsightsService = progressInsightsService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var dashboardViewModel = new AdminDashboardViewModel
            {
                TotalUsers = _userManager.Users.Count(),
                TotalWorkoutPrograms = _workoutProgramService.GetAll().Count(),
                TotalExercises = _exerciseService.GetAll().Count(),
                TotalMealPlans = _userManager.Users.SelectMany(u => _mealPlanService.GetByUser(u.Id)).Count(),
                RecentUsers = await _userManager.Users.Take(5).ToListAsync(),
                SystemStats = await GetSystemStats()
            };

            return View(dashboardViewModel);
        }

        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users.ToListAsync();
            var userViewModels = _mapper.Map<List<AdminUserViewModel>>(users);
            
            return View(userViewModels);
        }

        public async Task<IActionResult> UserDetails(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var userDetails = new AdminUserDetailsViewModel
            {
                User = user,
                WorkoutPrograms = _workoutProgramService.GetAll().Where(w => w.UserId == id).ToList(),
                MealPlans = _mealPlanService.GetByUser(id).ToList(),
                ProgressLogs = _measurementLogService.GetByUser(id).ToList(),
                ProgressInsights = _progressInsightsService.GetInsights(id)
            };

            return View(userDetails);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleUserStatus(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            user.IsBanned = !user.IsBanned;
            await _userManager.UpdateAsync(user);

            TempData["Success"] = $"User {user.Email} status updated successfully.";
            return RedirectToAction(nameof(Users));
        }

        public IActionResult SystemStats()
        {
            var stats = new SystemStatsViewModel
            {
                TotalUsers = _userManager.Users.Count(),
                ActiveUsers = _userManager.Users.Count(u => !u.IsBanned),
                TotalWorkouts = _workoutProgramService.GetAll().Count(),
                TotalExercises = _exerciseService.GetAll().Count(),
                TotalMealPlans = _userManager.Users.SelectMany(u => _mealPlanService.GetByUser(u.Id)).Count(),
                TotalProgressLogs = _userManager.Users.SelectMany(u => _measurementLogService.GetByUser(u.Id)).Count()
            };

            return View(stats);
        }

        private async Task<SystemStatsViewModel> GetSystemStats()
        {
            return new SystemStatsViewModel
            {
                TotalUsers = _userManager.Users.Count(),
                ActiveUsers = _userManager.Users.Count(u => !u.IsBanned),
                TotalWorkouts = _workoutProgramService.GetAll().Count(),
                TotalExercises = _exerciseService.GetAll().Count(),
                TotalMealPlans = _userManager.Users.SelectMany(u => _mealPlanService.GetByUser(u.Id)).Count(),
                TotalProgressLogs = _userManager.Users.SelectMany(u => _measurementLogService.GetByUser(u.Id)).Count()
            };
        }
    }
} 