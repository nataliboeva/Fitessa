using Xunit;
using Moq;
using Fitessa.Services.Services;
using Fitessa.Services.Interfaces;
using Fitessa.Data;
using Microsoft.EntityFrameworkCore;
using Fitessa.Data.Entities;

namespace Fitessa.Tests.Services
{
    public class MealPlanServiceTests
    {
        private readonly ApplicationDbContext _context;
        private readonly MealPlanService _service;

        public MealPlanServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _service = new MealPlanService(_context);
        }

        [Fact]
        public void GetByUser_WithValidUserId_ReturnsUserMealPlans()
        {
            var userId = "test-user-id";
            var mealPlans = new List<MealPlan>
            {
                new MealPlan { Id = 1, UserId = userId, Name = "Breakfast Plan", Description = "Healthy breakfast options" },
                new MealPlan { Id = 2, UserId = userId, Name = "Lunch Plan", Description = "Balanced lunch meals" }
            };

            _context.MealPlans.AddRange(mealPlans);
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
        public void Create_WithValidMealPlan_AddsToDatabase()
        {
            var mealPlan = new MealPlan
            {
                UserId = "test-user",
                Name = "Test Meal Plan",
                Description = "Test description"
            };

            _service.Create(mealPlan);

            var savedMealPlan = _context.MealPlans.FirstOrDefault(m => m.Name == "Test Meal Plan");
            Assert.NotNull(savedMealPlan);
            Assert.Equal("test-user", savedMealPlan.UserId);
        }

        [Fact]
        public void GetById_WithValidId_ReturnsMealPlan()
        {
            var mealPlan = new MealPlan
            {
                Id = 1,
                UserId = "test-user",
                Name = "Test Plan",
                Description = "Test Description"
            };

            _context.MealPlans.Add(mealPlan);
            _context.SaveChanges();

            var result = _service.GetById(1);

            Assert.NotNull(result);
            Assert.Equal("Test Plan", result.Name);
        }

        [Fact]
        public void GetById_WithInvalidId_ReturnsNull()
        {
            var result = _service.GetById(999);

            Assert.Null(result);
        }
    }
} 