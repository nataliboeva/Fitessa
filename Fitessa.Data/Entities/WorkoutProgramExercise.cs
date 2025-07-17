namespace Fitessa.Data.Entities
{
    public class WorkoutProgramExercise
    {
        public int WorkoutProgramId { get; set; }
        public int ExerciseId { get; set; }
        public int OrderIndex { get; set; }
        public int Reps { get; set; }
        public int Sets { get; set; }
        public WorkoutProgram WorkoutProgram { get; set; }
        public Exercise Exercise { get; set; }
    }
} 