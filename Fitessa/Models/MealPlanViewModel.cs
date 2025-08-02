using System.ComponentModel.DataAnnotations;

namespace Fitessa.Models
{
    public class MealPlanViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Range(1, 5000)]
        public int CaloriesPerDay { get; set; }

        [StringLength(20)]
        public string DietType { get; set; } = string.Empty;
    }
} 