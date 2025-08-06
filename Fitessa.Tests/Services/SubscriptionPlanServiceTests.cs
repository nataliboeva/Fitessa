using Xunit;
using Moq;
using Fitessa.Services.Services;
using Fitessa.Services.Interfaces;
using Fitessa.Data;
using Microsoft.EntityFrameworkCore;
using Fitessa.Data.Entities;

namespace Fitessa.Tests.Services
{
    public class SubscriptionPlanServiceTests
    {
        private readonly ApplicationDbContext _context;
        private readonly SubscriptionPlanService _service;

        public SubscriptionPlanServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _service = new SubscriptionPlanService(_context);
        }

        [Fact]
        public void GetAll_ReturnsAllActivePlans()
        {
            var plans = new List<SubscriptionPlan>
            {
                new SubscriptionPlan { Id = 1, Name = "Free", Price = 0, IsActive = true },
                new SubscriptionPlan { Id = 2, Name = "Monthly", Price = 19.99m, IsActive = true },
                new SubscriptionPlan { Id = 3, Name = "Yearly", Price = 199.99m, IsActive = true }
            };

            _context.SubscriptionPlans.AddRange(plans);
            _context.SaveChanges();

            var result = _service.GetAll();

            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public void GetById_WithValidId_ReturnsPlan()
        {
            var plan = new SubscriptionPlan
            {
                Id = 1,
                Name = "Test Plan",
                Description = "Test Description",
                Price = 29.99m,
                DurationDays = 30,
                IsActive = true
            };

            _context.SubscriptionPlans.Add(plan);
            _context.SaveChanges();

            var result = _service.GetById(1);

            Assert.NotNull(result);
            Assert.Equal("Test Plan", result.Name);
            Assert.Equal(29.99m, result.Price);
        }

        [Fact]
        public void GetById_WithInvalidId_ReturnsNull()
        {
            var result = _service.GetById(999);

            Assert.Null(result);
        }

        [Fact]
        public void Create_WithValidPlan_AddsToDatabase()
        {
            var plan = new SubscriptionPlan
            {
                Name = "New Plan",
                Description = "New Description",
                Price = 49.99m,
                DurationDays = 60,
                IsActive = true
            };

            _service.Create(plan);

            var savedPlan = _context.SubscriptionPlans.FirstOrDefault(p => p.Name == "New Plan");
            Assert.NotNull(savedPlan);
            Assert.Equal(49.99m, savedPlan.Price);
        }

        [Fact]
        public void Update_WithValidPlan_UpdatesDatabase()
        {
            var plan = new SubscriptionPlan
            {
                Id = 1,
                Name = "Original Plan",
                Price = 19.99m,
                IsActive = true
            };

            _context.SubscriptionPlans.Add(plan);
            _context.SaveChanges();

            plan.Name = "Updated Plan";
            plan.Price = 29.99m;

            _service.Update(plan);

            var updatedPlan = _context.SubscriptionPlans.Find(1);
            Assert.Equal("Updated Plan", updatedPlan.Name);
            Assert.Equal(29.99m, updatedPlan.Price);
        }
    }
} 