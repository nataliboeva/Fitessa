using System;

namespace Fitessa.Models.Entities
{
    public class UserSubscription
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int SubscriptionPlanId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public string RenewalType { get; set; }
        public ApplicationUser User { get; set; }
        public SubscriptionPlan SubscriptionPlan { get; set; }
    }
} 