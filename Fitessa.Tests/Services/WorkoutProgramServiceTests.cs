using Xunit;
using Moq;
using Fitessa.Services.Services;
using Fitessa.Services.Interfaces;
using Fitessa.Data;
using Microsoft.EntityFrameworkCore;
using Fitessa.Data.Entities;

namespace Fitessa.Tests.Services
{
    public class WorkoutProgramServiceTests
    {
        private readonly Mock<ApplicationDbContext> _mockContext;
        private readonly WorkoutProgramService _service;

        public WorkoutProgramServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _mockContext = new Mock<ApplicationDbContext>(options);
            _service = new WorkoutProgramService(_mockContext.Object);
        }

        [Fact]
        public async Task GetAllWorkoutProgramsAsync_ReturnsAllPrograms()
        {
            var programs = new List<WorkoutProgram>
            {
                new WorkoutProgram { Id = 1, Name = "Beginner Program", Difficulty = "Beginner" },
                new WorkoutProgram { Id = 2, Name = "Advanced Program", Difficulty = "Advanced" }
            };

            var mockSet = programs.AsQueryable().BuildMockDbSet();
            _mockContext.Setup(c => c.WorkoutPrograms).Returns(mockSet.Object);

            var result = await _service.GetAllWorkoutProgramsAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetWorkoutProgramByIdAsync_WithValidId_ReturnsProgram()
        {
            var program = new WorkoutProgram { Id = 1, Name = "Test Program" };
            var mockSet = new List<WorkoutProgram> { program }.AsQueryable().BuildMockDbSet();
            _mockContext.Setup(c => c.WorkoutPrograms).Returns(mockSet.Object);

            var result = await _service.GetWorkoutProgramByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal("Test Program", result.Name);
        }

        [Fact]
        public async Task GetWorkoutProgramByIdAsync_WithInvalidId_ReturnsNull()
        {
            var mockSet = new List<WorkoutProgram>().AsQueryable().BuildMockDbSet();
            _mockContext.Setup(c => c.WorkoutPrograms).Returns(mockSet.Object);

            var result = await _service.GetWorkoutProgramByIdAsync(999);

            Assert.Null(result);
        }
    }

    public static class MockDbSetExtensions
    {
        public static Mock<DbSet<T>> BuildMockDbSet<T>(this IQueryable<T> data) where T : class
        {
            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            return mockSet;
        }
    }
} 