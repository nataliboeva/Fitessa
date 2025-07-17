using System;

namespace Fitessa.Data.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int SubscriptionPlanId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaidAt { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentProvider { get; set; }
        public string TransactionId { get; set; }
        public ApplicationUser User { get; set; }
        public SubscriptionPlan SubscriptionPlan { get; set; }
    }
} 