using System;

namespace Fitessa.Data.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public required string UserId { get; set; }
        public required string Type { get; set; }
        public required string Title { get; set; }
        public required string Message { get; set; }
        public DateTime DeliveryTime { get; set; }
        public required ApplicationUser User { get; set; }
    }
} 