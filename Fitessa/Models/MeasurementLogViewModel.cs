using System.ComponentModel.DataAnnotations;

namespace Fitessa.Models
{
    public class MeasurementLogViewModel
    {
        public int Id { get; set; }
        
        public string UserId { get; set; } = string.Empty;
        
        [Required]
        public DateTime LoggedAt { get; set; }
        
        public decimal? WeightKg { get; set; }
        
        public decimal? BodyFatPercentage { get; set; }
        
        public decimal? MuscleMass { get; set; }
        
        public decimal? ChestCircumference { get; set; }
        
        public decimal? WaistCircumference { get; set; }
        
        public decimal? HipCircumference { get; set; }
        
        public decimal? BicepCircumference { get; set; }
        
        public decimal? ThighCircumference { get; set; }
        
        public decimal? CalfCircumference { get; set; }
        
        public string Notes { get; set; } = string.Empty;
        
        public string ImageUrl { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime? UpdatedAt { get; set; }
    }
} 