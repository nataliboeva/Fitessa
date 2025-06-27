using System.Collections.Generic;

namespace Fitessa.Models.Entities
{
    public class SubscriptionPlan
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public long DurationDays { get; set; }
        public bool IsActive { get; set; }
        public ICollection<UserSubscription> UserSubscriptions { get; set; }
        public ICollection<Payment> Payments { get; set; }
    }
} 