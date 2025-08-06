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
    public class ExerciseServiceTests
    {
        private readonly ApplicationDbContext _context;
        private readonly ExerciseService _service;

        public ExerciseServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _service = new ExerciseService(_context);
        }

        [Fact]
        public void GetAll_ReturnsAllExercises()
        {
            var exercises = new List<Exercise>
            {
                new Exercise { Id = 1, Name = "Push Up", MuscleGroup = "Chest", DifficultyLevel = "Beginner", Description = "Test" },
                new Exercise { Id = 2, Name = "Squat", MuscleGroup = "Legs", DifficultyLevel = "Beginner", Description = "Test" },
                new Exercise { Id = 3, Name = "Pull Up", MuscleGroup = "Back", DifficultyLevel = "Intermediate", Description = "Test" }
            };

            _context.Exercises.AddRange(exercises);
            _context.SaveChanges();

            var result = _service.GetAll();

            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public void GetAll_WithNoExercises_ReturnsEmptyList()
        {
            var result = _service.GetAll();

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void GetAll_ReturnsCorrectExerciseData()
        {
            var exercise = new Exercise 
            { 
                Id = 1, 
                Name = "Bench Press", 
                Description = "Chest exercise with barbell",
                MuscleGroup = "Chest", 
                DifficultyLevel = "Intermediate" 
            };

            _context.Exercises.Add(exercise);
            _context.SaveChanges();

            var result = _service.GetAll().First();

            Assert.Equal("Bench Press", result.Name);
            Assert.Equal("Chest exercise with barbell", result.Description);
            Assert.Equal("Chest", result.MuscleGroup);
            Assert.Equal("Intermediate", result.DifficultyLevel);
        }
    }
} 