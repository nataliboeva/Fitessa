using System.ComponentModel.DataAnnotations;

namespace Fitessa.Models
{
    public class SubscriptionPlanViewModel
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;
        
        [Required]
        [Range(0, 1000)]
        public decimal Price { get; set; }
        
        [Required]
        public string Duration { get; set; } = string.Empty;
        
        public string Features { get; set; } = string.Empty;
        
        public bool IsActive { get; set; }
        
        public bool IsPopular { get; set; }
        
        public string Color { get; set; } = string.Empty;
        
        public string Icon { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime? UpdatedAt { get; set; }
    }
} 