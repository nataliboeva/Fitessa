using System.ComponentModel.DataAnnotations;

namespace Fitessa.Models
{
    public class MeasurementLogViewModel
    {
        public int Id { get; set; }

        [Required]
        [Range(30, 300)]
        public decimal WeightKg { get; set; }

        [Required]
        [Range(100, 250)]
        public decimal HeightCm { get; set; }

        [Range(0, 100)]
        public decimal? BodyFatPercentage { get; set; }

        [Range(0, 200)]
        public decimal? MuscleMassKg { get; set; }

        public DateTime LoggedAt { get; set; } = DateTime.Now;

        public string UserId { get; set; } = string.Empty;
    }
} 