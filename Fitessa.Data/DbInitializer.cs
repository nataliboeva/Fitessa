using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Fitessa.Data.Entities;
using Microsoft.AspNetCore.Identity;

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
                    new Exercise { Name = "Squat", Description = "Bodyweight leg exercise", MuscleGroup = "Legs", DifficultyLevel = "Beginner" },
                    new Exercise { Name = "Pull Up", Description = "Upper body pulling exercise", MuscleGroup = "Back", DifficultyLevel = "Intermediate" },
                    new Exercise { Name = "Bench Press", Description = "Chest press with barbell", MuscleGroup = "Chest", DifficultyLevel = "Intermediate" },
                    new Exercise { Name = "Deadlift", Description = "Full body barbell lift", MuscleGroup = "Back", DifficultyLevel = "Advanced" },
                    new Exercise { Name = "Bicep Curl", Description = "Dumbbell bicep exercise", MuscleGroup = "Arms", DifficultyLevel = "Beginner" },
                    new Exercise { Name = "Tricep Dip", Description = "Bodyweight tricep exercise", MuscleGroup = "Arms", DifficultyLevel = "Beginner" },
                    new Exercise { Name = "Lunge", Description = "Leg exercise for quads and glutes", MuscleGroup = "Legs", DifficultyLevel = "Beginner" },
                    new Exercise { Name = "Plank", Description = "Core stability exercise", MuscleGroup = "Core", DifficultyLevel = "Beginner" },
                    new Exercise { Name = "Shoulder Press", Description = "Dumbbell shoulder exercise", MuscleGroup = "Shoulders", DifficultyLevel = "Intermediate" }
                );
            }

            context.SaveChanges();

            // Seed Identity Roles and Users
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Seed Admin Role
            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                roleManager.CreateAsync(new IdentityRole("Admin")).Wait();
            }
            if (!roleManager.RoleExistsAsync("User").Result)
            {
                roleManager.CreateAsync(new IdentityRole("User")).Wait();
            }

            // Seed Admin User
            var adminEmail = "admin@fitessa.com";
            var adminUser = userManager.FindByEmailAsync(adminEmail).Result;
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Admin",
                    LastName = "User",
                    EmailConfirmed = true,
                    Gender = "Other",
                    ProfilePictureUrl = "/images/default-profile.png" // Added ProfilePictureUrl
                };
                userManager.CreateAsync(adminUser, "Admin123!").Wait();
                userManager.AddToRoleAsync(adminUser, "Admin").Wait();
            }

            // Seed Regular Users
            for (int i = 1; i <= 3; i++)
            {
                var email = $"user{i}@fitessa.com";
                if (userManager.FindByEmailAsync(email).Result == null)
                {
                    var user = new ApplicationUser
                    {
                        UserName = email,
                        Email = email,
                        FirstName = $"User{i}",
                        LastName = "Test",
                        EmailConfirmed = true,
                        Gender = "Other",
                        ProfilePictureUrl = "/images/default-profile.png" // Added ProfilePictureUrl
                    };
                    userManager.CreateAsync(user, $"User{i}123!").Wait();
                    userManager.AddToRoleAsync(user, "User").Wait();
                }
            }
        }
    }
} 