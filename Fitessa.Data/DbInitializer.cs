using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Fitessa.Data.Entities;

namespace Fitessa.Data
{
    public static class DbInitializer
    {
        public static void Seed(IServiceProvider serviceProvider)
        {
            using var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

            // Seed Subscription Plans
            if (!context.SubscriptionPlans.Any())
            {
                context.SubscriptionPlans.AddRange(
                    new SubscriptionPlan { Name = "Free", Price = 0, DurationDays = 0, IsActive = true },
                    new SubscriptionPlan { Name = "Monthly", Price = 19.99m, DurationDays = 30, IsActive = true },
                    new SubscriptionPlan { Name = "Yearly", Price = 199.99m, DurationDays = 365, IsActive = true }
                );
            }

            // Seed Exercises
            if (!context.Exercises.Any())
            {
                context.Exercises.AddRange(
                    new Exercise { Name = "Push Up", Description = "Bodyweight chest exercise", MuscleGroup = "Chest", DifficultyLevel = "Beginner" },
                    new Exercise { Name = "Squat", Description = "Bodyweight leg exercise", MuscleGroup = "Legs", DifficultyLevel = "Beginner" }
                );
            }

            context.SaveChanges();
        }
    }
} 