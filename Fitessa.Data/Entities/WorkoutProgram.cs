using System.Collections.Generic;

namespace Fitessa.Data.Entities
{
    public class WorkoutProgram
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string UserId { get; set; }
        public required string Difficulty { get; set; }
        public int DurationDays { get; set; }
        public required ApplicationUser User { get; set; }
        public ICollection<WorkoutProgramExercise> WorkoutProgramExercises { get; set; } = new List<WorkoutProgramExercise>();
    }
} 