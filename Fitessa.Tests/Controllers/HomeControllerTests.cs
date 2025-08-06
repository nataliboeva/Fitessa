using Microsoft.AspNetCore.Mvc;
using Xunit;
using Fitessa.Web.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Fitessa.Data.Entities;
using Fitessa.Services.Interfaces;
using Moq;
using System.Threading.Tasks;

namespace Fitessa.Tests.Controllers
{
    public class HomeControllerTests
    {
        private readonly Mock<ILogger<HomeController>> _mockLogger;
        private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
        private readonly Mock<IWorkoutProgramService> _mockWorkoutProgramService;
        private readonly Mock<IMealPlanService> _mockMealPlanService;
        private readonly Mock<IMeasurementLogService> _mockMeasurementLogService;
        private readonly Mock<IProgressInsightsService> _mockProgressInsightsService;

        public HomeControllerTests()
        {
            _mockLogger = new Mock<ILogger<HomeController>>();
            _mockUserManager = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
            _mockWorkoutProgramService = new Mock<IWorkoutProgramService>();
            _mockMealPlanService = new Mock<IMealPlanService>();
            _mockMeasurementLogService = new Mock<IMeasurementLogService>();
            _mockProgressInsightsService = new Mock<IProgressInsightsService>();
        }

        [Fact]
        public async Task Index_ReturnsViewResult()
        {
            var controller = new HomeController(
                _mockLogger.Object,
                _mockUserManager.Object,
                _mockWorkoutProgramService.Object,
                _mockMealPlanService.Object,
                _mockMeasurementLogService.Object,
                _mockProgressInsightsService.Object);

            var result = await controller.Index();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Privacy_ReturnsViewResult()
        {
            var controller = new HomeController(
                _mockLogger.Object,
                _mockUserManager.Object,
                _mockWorkoutProgramService.Object,
                _mockMealPlanService.Object,
                _mockMeasurementLogService.Object,
                _mockProgressInsightsService.Object);

            var result = controller.Privacy();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Error_ReturnsViewResult()
        {
            var controller = new HomeController(
                _mockLogger.Object,
                _mockUserManager.Object,
                _mockWorkoutProgramService.Object,
                _mockMealPlanService.Object,
                _mockMeasurementLogService.Object,
                _mockProgressInsightsService.Object);

            var result = controller.Error();

            Assert.IsType<ViewResult>(result);
        }
    }
} 