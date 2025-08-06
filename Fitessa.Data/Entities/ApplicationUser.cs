using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Fitessa.Data.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string ProfilePictureUrl { get; set; } = string.Empty;
        public bool IsPremium { get; set; }
        public bool IsBanned { get; set; }
        public DateTime LastLogin { get; set; }
        public string Goal { get; set; } = string.Empty;
        public string? GoalType { get; set; }
        public decimal? GoalValue { get; set; }
        public ICollection<MeasurementLog> MeasurementLogs { get; set; } = new List<MeasurementLog>();
        public ICollection<WorkoutSession> WorkoutSessions { get; set; } = new List<WorkoutSession>();
        public ICollection<UserSubscription> UserSubscriptions { get; set; } = new List<UserSubscription>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
} 