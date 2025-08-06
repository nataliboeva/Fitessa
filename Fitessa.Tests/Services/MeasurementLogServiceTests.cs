using Xunit;
using Moq;
using Fitessa.Services.Services;
using Fitessa.Services.Interfaces;
using Fitessa.Data;
using Microsoft.EntityFrameworkCore;
using Fitessa.Data.Entities;

namespace Fitessa.Tests.Services
{
    public class MeasurementLogServiceTests
    {
        private readonly ApplicationDbContext _context;
        private readonly MeasurementLogService _service;

        public MeasurementLogServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _service = new MeasurementLogService(_context);
        }

        [Fact]
        public void GetByUser_WithValidUserId_ReturnsUserLogs()
        {
            var userId = "test-user-id";
            var logs = new List<MeasurementLog>
            {
                new MeasurementLog { Id = 1, UserId = userId, Weight = 70.5, Date = DateTime.Now.AddDays(-1) },
                new MeasurementLog { Id = 2, UserId = userId, Weight = 70.0, Date = DateTime.Now }
            };

            _context.MeasurementLogs.AddRange(logs);
            _context.SaveChanges();

            var result = _service.GetByUser(userId);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void GetByUser_WithInvalidUserId_ReturnsEmptyList()
        {
            var result = _service.GetByUser("invalid-user-id");

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void Create_WithValidLog_AddsToDatabase()
        {
            var log = new MeasurementLog
            {
                UserId = "test-user",
                Weight = 75.5,
                Height = 180.0,
                Date = DateTime.Now
            };

            _service.Create(log);

            var savedLog = _context.MeasurementLogs.FirstOrDefault(m => m.UserId == "test-user");
            Assert.NotNull(savedLog);
            Assert.Equal(75.5, savedLog.Weight);
        }

        [Fact]
        public void GetById_WithValidId_ReturnsLog()
        {
            var log = new MeasurementLog
            {
                Id = 1,
                UserId = "test-user",
                Weight = 70.0,
                Height = 175.0,
                Date = DateTime.Now
            };

            _context.MeasurementLogs.Add(log);
            _context.SaveChanges();

            var result = _service.GetById(1);

            Assert.NotNull(result);
            Assert.Equal(70.0, result.Weight);
        }

        [Fact]
        public void GetById_WithInvalidId_ReturnsNull()
        {
            var result = _service.GetById(999);

            Assert.Null(result);
        }
    }
} 