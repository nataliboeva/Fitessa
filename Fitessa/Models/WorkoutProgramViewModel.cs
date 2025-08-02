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
        [StringLength(20)]
        public string Difficulty { get; set; } = string.Empty;

        [Range(1, 365)]
        public int DurationDays { get; set; }

        public int ExerciseCount { get; set; }
    }
} 