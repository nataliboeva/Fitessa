using System;

namespace Fitessa.Models.Entities
{
    public class MeasurementLog
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime LoggedAt { get; set; }
        public decimal WeightKg { get; set; }
        public int HeightCm { get; set; }
        public ApplicationUser User { get; set; }
    }
} 