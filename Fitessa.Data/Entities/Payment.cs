using System;

namespace Fitessa.Data.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public required string UserId { get; set; }
        public int SubscriptionPlanId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaidAt { get; set; }
        public required string PaymentStatus { get; set; }
        public required string PaymentProvider { get; set; }
        public required string TransactionId { get; set; }
        public required ApplicationUser User { get; set; }
        public required SubscriptionPlan SubscriptionPlan { get; set; }
    }
} 