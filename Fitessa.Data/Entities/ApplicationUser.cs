using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Fitessa.Data.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string ProfilePictureUrl { get; set; }
        public bool IsPremium { get; set; }
        public bool IsBanned { get; set; }
        public DateTime LastLogin { get; set; }
        public ICollection<MeasurementLog> MeasurementLogs { get; set; }
        public ICollection<WorkoutSession> WorkoutSessions { get; set; }
        public ICollection<UserSubscription> UserSubscriptions { get; set; }
        public ICollection<Notification> Notifications { get; set; }
        public ICollection<Payment> Payments { get; set; }
    }
} 