using System.ComponentModel.DataAnnotations;

namespace Fitessa.Models
{
    public class WorkoutProgramViewModel
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;
        
        [Required]
        public string Difficulty { get; set; } = string.Empty;
        
        public int Duration { get; set; }
        
        public string Goal { get; set; } = string.Empty;
        
        public bool IsActive { get; set; }
        
        public int ExerciseCount { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime? UpdatedAt { get; set; }
    }
} 