using System.Collections.Generic;

namespace Fitessa.Models.Entities
{
    public class Exercise
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string MuscleGroup { get; set; }
        public string DifficultyLevel { get; set; }
        public ICollection<WorkoutProgramExercise> WorkoutProgramExercises { get; set; }
    }
} 