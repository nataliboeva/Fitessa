using Microsoft.AspNetCore.Mvc;
using Xunit;
using Fitessa.Controllers;
using Fitessa.Services.Interfaces;
using Fitessa.Data.Entities;
using Moq;
using DinkToPdf.Contracts;

namespace Fitessa.Tests.Controllers
{
    public class WorkoutProgramsControllerTests
    {
        private readonly Mock<IWorkoutProgramService> _mockWorkoutProgramService;
        private readonly Mock<IExerciseService> _mockExerciseService;
        private readonly Mock<IConverter> _mockPdfConverter;

        public WorkoutProgramsControllerTests()
        {
            _mockWorkoutProgramService = new Mock<IWorkoutProgramService>();
            _mockExerciseService = new Mock<IExerciseService>();
            _mockPdfConverter = new Mock<IConverter>();
        }

        [Fact]
        public void Index_ReturnsViewResult()
        {
            var programs = new List<WorkoutProgram>
            {
                new WorkoutProgram { Id = 1, Name = "Beginner Program", Difficulty = "Beginner" },
                new WorkoutProgram { Id = 2, Name = "Advanced Program", Difficulty = "Advanced" }
            };

            _mockWorkoutProgramService.Setup(x => x.GetAll()).Returns(programs);

            var controller = new WorkoutProgramsController(
                _mockWorkoutProgramService.Object,
                _mockExerciseService.Object,
                _mockPdfConverter.Object);

            var result = controller.Index();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Details_WithValidId_ReturnsViewResult()
        {
            var program = new WorkoutProgram { Id = 1, Name = "Test Program", Difficulty = "Intermediate" };
            var exercises = new List<Exercise>
            {
                new Exercise { Id = 1, Name = "Push Up", MuscleGroup = "Chest" }
            };

            _mockWorkoutProgramService.Setup(x => x.GetById(1)).Returns(program);
            _mockExerciseService.Setup(x => x.GetAll()).Returns(exercises);

            var controller = new WorkoutProgramsController(
                _mockWorkoutProgramService.Object,
                _mockExerciseService.Object,
                _mockPdfConverter.Object);

            var result = controller.Details(1);

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Details_WithInvalidId_ReturnsNotFound()
        {
            _mockWorkoutProgramService.Setup(x => x.GetById(999)).Returns((WorkoutProgram)null);

            var controller = new WorkoutProgramsController(
                _mockWorkoutProgramService.Object,
                _mockExerciseService.Object,
                _mockPdfConverter.Object);

            var result = controller.Details(999);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Create_Get_ReturnsViewResult()
        {
            var controller = new WorkoutProgramsController(
                _mockWorkoutProgramService.Object,
                _mockExerciseService.Object,
                _mockPdfConverter.Object);

            var result = controller.Create();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Create_Post_WithValidModel_RedirectsToIndex()
        {
            var program = new WorkoutProgram { Name = "Test Program", Difficulty = "Beginner" };

            var controller = new WorkoutProgramsController(
                _mockWorkoutProgramService.Object,
                _mockExerciseService.Object,
                _mockPdfConverter.Object);

            var result = controller.Create(program);

            Assert.IsType<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.Equal("Index", redirectResult.ActionName);
        }

        [Fact]
        public void Edit_WithValidId_ReturnsViewResult()
        {
            var program = new WorkoutProgram { Id = 1, Name = "Test Program", Difficulty = "Intermediate" };

            _mockWorkoutProgramService.Setup(x => x.GetById(1)).Returns(program);

            var controller = new WorkoutProgramsController(
                _mockWorkoutProgramService.Object,
                _mockExerciseService.Object,
                _mockPdfConverter.Object);

            var result = controller.Edit(1);

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Edit_WithInvalidId_ReturnsNotFound()
        {
            _mockWorkoutProgramService.Setup(x => x.GetById(999)).Returns((WorkoutProgram)null);

            var controller = new WorkoutProgramsController(
                _mockWorkoutProgramService.Object,
                _mockExerciseService.Object,
                _mockPdfConverter.Object);

            var result = controller.Edit(999);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Delete_WithValidId_ReturnsViewResult()
        {
            var program = new WorkoutProgram { Id = 1, Name = "Test Program", Difficulty = "Intermediate" };

            _mockWorkoutProgramService.Setup(x => x.GetById(1)).Returns(program);

            var controller = new WorkoutProgramsController(
                _mockWorkoutProgramService.Object,
                _mockExerciseService.Object,
                _mockPdfConverter.Object);

            var result = controller.Delete(1);

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Delete_WithInvalidId_ReturnsNotFound()
        {
            _mockWorkoutProgramService.Setup(x => x.GetById(999)).Returns((WorkoutProgram)null);

            var controller = new WorkoutProgramsController(
                _mockWorkoutProgramService.Object,
                _mockExerciseService.Object,
                _mockPdfConverter.Object);

            var result = controller.Delete(999);

            Assert.IsType<NotFoundResult>(result);
        }
    }
} 