using System.Collections.Generic;

namespace Fitessa.Data.Entities
{
    public class Exercise
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string MuscleGroup { get; set; }
        public required string DifficultyLevel { get; set; }
        public ICollection<WorkoutProgramExercise> WorkoutProgramExercises { get; set; } = new List<WorkoutProgramExercise>();
    }
} 