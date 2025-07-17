using System.Collections.Generic;

namespace Fitessa.Data.Entities
{
    public class WorkoutProgram
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public string Difficulty { get; set; }
        public int DurationDays { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<WorkoutProgramExercise> WorkoutProgramExercises { get; set; }
    }
} 