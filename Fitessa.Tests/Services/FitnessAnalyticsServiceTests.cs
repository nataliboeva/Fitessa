using Xunit;
using Moq;
using Fitessa.Services.Services;
using Fitessa.Services.Interfaces;
using Fitessa.Data;
using Microsoft.EntityFrameworkCore;
using Fitessa.Data.Entities;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Fitessa.Tests.Services
{
    public class FitnessAnalyticsServiceTests
    {
        private readonly ApplicationDbContext _context;
        private readonly FitnessAnalyticsService _service;

        public FitnessAnalyticsServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _service = new FitnessAnalyticsService(_context, new Mock<ILogger<FitnessAnalyticsService>>().Object);
        }

        [Fact]
        public void CalculateBMI_WithValidParameters_ReturnsCorrectBMI()
        {
            var weightKg = 70.0;
            var heightCm = 175.0;
            var expectedBMI = 22.86;

            var result = _service.CalculateBMI(weightKg, heightCm);

            Assert.Equal(expectedBMI, result, 2);
        }

        [Fact]
        public void GetBMICategory_WithNormalBMI_ReturnsNormal()
        {
            var bmi = 22.0;

            var result = _service.GetBMICategory(bmi);

            Assert.Equal("Normal", result);
        }

        [Fact]
        public void GetBMICategory_WithOverweightBMI_ReturnsOverweight()
        {
            var bmi = 27.0;

            var result = _service.GetBMICategory(bmi);

            Assert.Equal("Overweight", result);
        }

        [Fact]
        public void CalculateBMR_WithValidParameters_ReturnsCorrectBMR()
        {
            var weightKg = 70.0;
            var heightCm = 175.0;
            var age = 30;
            var gender = "Male";
            var expectedBMR = 1653.5;

            var result = _service.CalculateBMR(weightKg, heightCm, age, gender);

            Assert.Equal(expectedBMR, result, 1);
        }

        [Fact]
        public void CalculateTDEE_WithSedentaryActivity_ReturnsCorrectTDEE()
        {
            var bmr = 1653.5;
            var activityLevel = "sedentary";
            var expectedTDEE = 1984.2;

            var result = _service.CalculateTDEE(bmr, activityLevel);

            Assert.Equal(expectedTDEE, result, 1);
        }

        [Fact]
        public void CalculateProgressPercentage_WithValidParameters_ReturnsCorrectPercentage()
        {
            var currentValue = 80.0;
            var targetValue = 100.0;
            var expectedPercentage = 80.0;

            var result = _service.CalculateProgressPercentage(currentValue, targetValue);

            Assert.Equal(expectedPercentage, result);
        }

        [Fact]
        public void GetProgressStatus_WithGoodProgress_ReturnsGood()
        {
            var progressPercentage = 75.0;

            var result = _service.GetProgressStatus(progressPercentage);

            Assert.Equal("Good", result);
        }

        [Fact]
        public void IsGoalAchieved_WithAchievedGoal_ReturnsTrue()
        {
            var currentValue = 100.0;
            var targetValue = 100.0;

            var result = _service.IsGoalAchieved(currentValue, targetValue);

            Assert.True(result);
        }

        [Fact]
        public void IsGoalAchieved_WithNotAchievedGoal_ReturnsFalse()
        {
            var currentValue = 80.0;
            var targetValue = 100.0;

            var result = _service.IsGoalAchieved(currentValue, targetValue);

            Assert.False(result);
        }

        [Fact]
        public void CalculateGoalProgress_WithValidParameters_ReturnsCorrectProgress()
        {
            var currentValue = 80.0;
            var targetValue = 100.0;
            var expectedProgress = 80.0;

            var result = _service.CalculateGoalProgress(currentValue, targetValue);

            Assert.Equal(expectedProgress, result);
        }

        [Fact]
        public void GenerateWorkoutRecommendations_WithValidParameters_ReturnsRecommendations()
        {
            var fitnessLevel = "Beginner";
            var goal = "Weight Loss";
            var currentWeight = 80.0;
            var targetWeight = 70.0;

            var result = _service.GenerateWorkoutRecommendations(fitnessLevel, goal, currentWeight, targetWeight);

            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public void GenerateNutritionRecommendations_WithValidParameters_ReturnsRecommendations()
        {
            var goal = "Weight Loss";
            var currentWeight = 80.0;
            var targetWeight = 70.0;

            var result = _service.GenerateNutritionRecommendations(goal, currentWeight, targetWeight);

            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public void CalculateBodyFatPercentage_WithValidParameters_ReturnsCorrectPercentage()
        {
            var bmi = 25.0;
            var age = 30;
            var gender = "Male";
            var expectedBodyFat = 20.0;

            var result = _service.CalculateBodyFatPercentage(bmi, age, gender);

            Assert.Equal(expectedBodyFat, result, 1);
        }

        [Fact]
        public void CalculateLeanBodyMass_WithValidParameters_ReturnsCorrectMass()
        {
            var weightKg = 80.0;
            var bodyFatPercentage = 20.0;
            var expectedLeanMass = 64.0;

            var result = _service.CalculateLeanBodyMass(weightKg, bodyFatPercentage);

            Assert.Equal(expectedLeanMass, result);
        }

        [Fact]
        public void CalculateWorkoutIntensity_WithValidExercises_ReturnsCorrectIntensity()
        {
            var exercises = new List<Exercise>
            {
                new Exercise { Name = "Push Up", DifficultyLevel = "Beginner", Description = "Test", MuscleGroup = "Chest" },
                new Exercise { Name = "Pull Up", DifficultyLevel = "Intermediate", Description = "Test", MuscleGroup = "Back" },
                new Exercise { Name = "Deadlift", DifficultyLevel = "Advanced", Description = "Test", MuscleGroup = "Back" }
            };
            var durationMinutes = 60;

            var result = _service.CalculateWorkoutIntensity(exercises, durationMinutes);

            Assert.True(result > 0);
        }

        [Fact]
        public void GenerateMilestones_WithValidParameters_ReturnsCorrectMilestones()
        {
            var startValue = 100.0;
            var targetValue = 80.0;
            var numberOfMilestones = 4;

            var result = _service.GenerateMilestones(startValue, targetValue, numberOfMilestones);

            Assert.NotNull(result);
            Assert.Equal(numberOfMilestones, result.Count);
        }

        [Fact]
        public void CalculateMacroRatio_WithValidParameters_ReturnsCorrectRatio()
        {
            var protein = 150.0;
            var carbs = 200.0;
            var fat = 50.0;
            var expectedRatio = 0.375;

            var result = _service.CalculateMacroRatio(protein, carbs, fat);

            Assert.Equal(expectedRatio, result, 3);
        }
    }
} 