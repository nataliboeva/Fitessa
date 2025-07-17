using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Fitessa.Data.Entities
{
    public class SubscriptionPlan
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public decimal Price { get; set; }
        public long DurationDays { get; set; }
        public bool IsActive { get; set; }
        public ICollection<UserSubscription> UserSubscriptions { get; set; } = new List<UserSubscription>();
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
} 