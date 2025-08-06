using System;

namespace Fitessa.Data.Entities
{
    public class MeasurementLog
    {
        public int Id { get; set; }
        public required string UserId { get; set; }
        public DateTime LoggedAt { get; set; }
        public decimal WeightKg { get; set; }
        public int HeightCm { get; set; }
        public required ApplicationUser User { get; set; }
    }
} 