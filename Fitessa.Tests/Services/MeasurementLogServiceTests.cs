using Xunit;
using Moq;
using Fitessa.Services.Services;
using Fitessa.Services.Interfaces;
using Fitessa.Data;
using Microsoft.EntityFrameworkCore;
using Fitessa.Data.Entities;
using System.Collections.Generic;

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
            var user = new ApplicationUser { Id = userId, UserName = "test@example.com", FirstName = "Test", LastName = "User", Email = "test@example.com", Gender = "Other", ProfilePictureUrl = "/images/default-profile.png" };
            var logs = new List<MeasurementLog>
            {
                new MeasurementLog { Id = 1, UserId = userId, WeightKg = 70.5m, LoggedAt = DateTime.Now.AddDays(-1), User = user },
                new MeasurementLog { Id = 2, UserId = userId, WeightKg = 70.0m, LoggedAt = DateTime.Now, User = user }
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
            var user = new ApplicationUser { Id = "test-user", UserName = "test@example.com", FirstName = "Test", LastName = "User", Email = "test@example.com", Gender = "Other", ProfilePictureUrl = "/images/default-profile.png" };
            var log = new MeasurementLog
            {
                UserId = "test-user",
                WeightKg = 75.5m,
                HeightCm = 180,
                LoggedAt = DateTime.Now,
                User = user
            };

            _service.Create(log);

            var savedLog = _context.MeasurementLogs.FirstOrDefault(m => m.UserId == "test-user");
            Assert.NotNull(savedLog);
            Assert.Equal(75.5m, savedLog.WeightKg);
        }

        [Fact]
        public void GetById_WithValidId_ReturnsLog()
        {
            var user = new ApplicationUser { Id = "test-user", UserName = "test@example.com", FirstName = "Test", LastName = "User", Email = "test@example.com", Gender = "Other", ProfilePictureUrl = "/images/default-profile.png" };
            var log = new MeasurementLog
            {
                Id = 1,
                UserId = "test-user",
                WeightKg = 70.0m,
                HeightCm = 175,
                LoggedAt = DateTime.Now,
                User = user
            };

            _context.MeasurementLogs.Add(log);
            _context.SaveChanges();

            var result = _service.GetById(1);

            Assert.NotNull(result);
            Assert.Equal(70.0m, result.WeightKg);
        }

        [Fact]
        public void GetById_WithInvalidId_ReturnsNull()
        {
            var result = _service.GetById(999);

            Assert.Null(result);
        }
    }
} 