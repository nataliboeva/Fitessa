using Xunit;
using Moq;
using Fitessa.Services.Services;
using Fitessa.Services.Interfaces;
using Fitessa.Data;
using Microsoft.EntityFrameworkCore;
using Fitessa.Data.Entities;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.EntityFrameworkCore;

namespace Fitessa.Tests.Services
{
    public class WorkoutProgramServiceTests
    {
        private readonly ApplicationDbContext _context;
        private readonly WorkoutProgramService _service;

        public WorkoutProgramServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _service = new WorkoutProgramService(_context);
        }

        [Fact]
        public void GetAll_ReturnsAllPrograms()
        {
            var programs = new List<WorkoutProgram>
            {
                new WorkoutProgram { Id = 1, Name = "Beginner Program", Difficulty = "Beginner", DurationDays = 30 },
                new WorkoutProgram { Id = 2, Name = "Advanced Program", Difficulty = "Advanced", DurationDays = 60 }
            };

            _context.WorkoutPrograms.AddRange(programs);
            _context.SaveChanges();

            var result = _service.GetAll();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void GetById_WithValidId_ReturnsProgram()
        {
            var program = new WorkoutProgram { Id = 1, Name = "Test Program", DurationDays = 30 };
            _context.WorkoutPrograms.Add(program);
            _context.SaveChanges();

            var result = _service.GetById(1);

            Assert.NotNull(result);
            Assert.Equal("Test Program", result.Name);
        }

        [Fact]
        public void GetById_WithInvalidId_ReturnsNull()
        {
            var result = _service.GetById(999);

            Assert.Null(result);
        }
    }


} 