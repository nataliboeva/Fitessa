using Microsoft.AspNetCore.Mvc;
using Xunit;
using Fitessa.Web.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Fitessa.Data.Entities;
using Fitessa.Services.Interfaces;
using Moq;
using AutoMapper;
using Fitessa.Models;
using System.Threading.Tasks;

namespace Fitessa.Tests.Controllers
{
    public class DashboardControllerTests
    {
        private readonly Mock<ILogger<DashboardController>> _mockLogger;
        private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
        private readonly Mock<IWorkoutProgramService> _mockWorkoutProgramService;
        private readonly Mock<IMealPlanService> _mockMealPlanService;
        private readonly Mock<IExerciseService> _mockExerciseService;
        private readonly Mock<IMeasurementLogService> _mockMeasurementLogService;
        private readonly Mock<IProgressInsightsService> _mockProgressInsightsService;
        private readonly Mock<IFitnessAnalyticsService> _mockFitnessAnalyticsService;
        private readonly Mock<IMapper> _mockMapper;

        public DashboardControllerTests()
        {
            _mockLogger = new Mock<ILogger<DashboardController>>();
            _mockUserManager = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
            _mockWorkoutProgramService = new Mock<IWorkoutProgramService>();
            _mockMealPlanService = new Mock<IMealPlanService>();
            _mockExerciseService = new Mock<IExerciseService>();
            _mockMeasurementLogService = new Mock<IMeasurementLogService>();
            _mockProgressInsightsService = new Mock<IProgressInsightsService>();
            _mockFitnessAnalyticsService = new Mock<IFitnessAnalyticsService>();
            _mockMapper = new Mock<IMapper>();
        }

        [Fact]
        public async Task Index_WithAuthenticatedUser_ReturnsViewResult()
        {
            var user = new ApplicationUser { Id = "test-user", UserName = "test@example.com", FirstName = "Test", LastName = "User", Email = "test@example.com", Gender = "Other", ProfilePictureUrl = "/images/default-profile.png" };
            _mockUserManager.Setup(x => x.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>()))
                .ReturnsAsync(user);

            var controller = new DashboardController(
                _mockUserManager.Object,
                _mockWorkoutProgramService.Object,
                _mockMealPlanService.Object,
                _mockExerciseService.Object,
                _mockMeasurementLogService.Object,
                _mockProgressInsightsService.Object,
                _mockFitnessAnalyticsService.Object,
                _mockMapper.Object);

            var result = await controller.Index();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Index_WithNullUser_ReturnsNotFound()
        {
            _mockUserManager.Setup(x => x.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>()))
                .ReturnsAsync((ApplicationUser)null);

            var controller = new DashboardController(
                _mockUserManager.Object,
                _mockWorkoutProgramService.Object,
                _mockMealPlanService.Object,
                _mockExerciseService.Object,
                _mockMeasurementLogService.Object,
                _mockProgressInsightsService.Object,
                _mockFitnessAnalyticsService.Object,
                _mockMapper.Object);

            var result = await controller.Index();

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Progress_WithValidUser_ReturnsViewResult()
        {
            var user = new ApplicationUser { Id = "test-user", UserName = "test@example.com", FirstName = "Test", LastName = "User", Email = "test@example.com", Gender = "Other", ProfilePictureUrl = "/images/default-profile.png" };
            _mockUserManager.Setup(x => x.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>()))
                .ReturnsAsync(user);

            var controller = new DashboardController(
                _mockUserManager.Object,
                _mockWorkoutProgramService.Object,
                _mockMealPlanService.Object,
                _mockExerciseService.Object,
                _mockMeasurementLogService.Object,
                _mockProgressInsightsService.Object,
                _mockFitnessAnalyticsService.Object,
                _mockMapper.Object);

            var result = await controller.Progress();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Analytics_WithValidUser_ReturnsViewResult()
        {
            var user = new ApplicationUser { Id = "test-user", UserName = "test@example.com", FirstName = "Test", LastName = "User", Email = "test@example.com", Gender = "Other", ProfilePictureUrl = "/images/default-profile.png" };
            _mockUserManager.Setup(x => x.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>()))
                .ReturnsAsync(user);

            var controller = new DashboardController(
                _mockUserManager.Object,
                _mockWorkoutProgramService.Object,
                _mockMealPlanService.Object,
                _mockExerciseService.Object,
                _mockMeasurementLogService.Object,
                _mockProgressInsightsService.Object,
                _mockFitnessAnalyticsService.Object,
                _mockMapper.Object);

            var result = await controller.Analytics();

            Assert.IsType<ViewResult>(result);
        }
    }
} 