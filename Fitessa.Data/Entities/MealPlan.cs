using System.ComponentModel.DataAnnotations;

namespace Fitessa.Data.Entities
{
    public class MealPlan
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string UserId { get; set; }
        public required ApplicationUser User { get; set; }
    }
} 