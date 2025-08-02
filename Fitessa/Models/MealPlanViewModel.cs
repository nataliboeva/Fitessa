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
        
        [Required]
        public string Category { get; set; } = string.Empty;
        
        public string Goal { get; set; } = string.Empty;
        
        public int Calories { get; set; }
        
        public decimal Protein { get; set; }
        
        public decimal Carbs { get; set; }
        
        public decimal Fat { get; set; }
        
        public string Instructions { get; set; } = string.Empty;
        
        public string Ingredients { get; set; } = string.Empty;
        
        public string ImageUrl { get; set; } = string.Empty;
        
        public bool IsActive { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime? UpdatedAt { get; set; }
    }
} 