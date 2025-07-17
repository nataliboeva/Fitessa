using System;

namespace Fitessa.Data.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime DeliveryTime { get; set; }
        public ApplicationUser User { get; set; }
    }
} 