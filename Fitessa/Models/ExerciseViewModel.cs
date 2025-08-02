using System.ComponentModel.DataAnnotations;

namespace Fitessa.Models
{
    public class ExerciseViewModel
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;
        
        [Required]
        public string Category { get; set; } = string.Empty;
        
        public string MuscleGroup { get; set; } = string.Empty;
        
        public string Equipment { get; set; } = string.Empty;
        
        public string Difficulty { get; set; } = string.Empty;
        
        public string Instructions { get; set; } = string.Empty;
        
        public string ImageUrl { get; set; } = string.Empty;
        
        public string VideoUrl { get; set; } = string.Empty;
        
        public bool IsActive { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime? UpdatedAt { get; set; }
    }
} 