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
        [Range(0, 10000)]
        public decimal Price { get; set; }

        [Required]
        [Range(1, 365)]
        public int DurationDays { get; set; }

        public bool IsActive { get; set; } = true;
    }
} 