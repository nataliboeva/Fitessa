using System;

namespace Fitessa.Data.Entities
{
    public class UserSubscription
    {
        public int Id { get; set; }
        public required string UserId { get; set; }
        public int SubscriptionPlanId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public required string Status { get; set; }
        public required string RenewalType { get; set; }
        public bool IsActive { get; set; }
        public required string PaymentId { get; set; }
        public required ApplicationUser User { get; set; }
        public required SubscriptionPlan SubscriptionPlan { get; set; }
    }
} 