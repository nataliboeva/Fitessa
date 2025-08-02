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
        [StringLength(50)]
        public string MuscleGroup { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string DifficultyLevel { get; set; } = string.Empty;
    }
} 