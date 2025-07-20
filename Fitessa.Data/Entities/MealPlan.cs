using System.ComponentModel.DataAnnotations;

namespace Fitessa.Data.Entities
{
    public class MealPlan
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
} 