using System;

namespace Fitessa.Data.Entities
{
    public class WorkoutSession
    {
        public int Id { get; set; }
        public required string UserId { get; set; }
        public int? WorkoutProgramId { get; set; }
        public required string Title { get; set; }
        public DateTime ScheduledTime { get; set; }
        public bool IsCompleted { get; set; }
        public int? DurationMin { get; set; }
        public required string Mood { get; set; }
        public int? FitnessCenterId { get; set; }
        public required ApplicationUser User { get; set; }
        public WorkoutProgram? WorkoutProgram { get; set; }
        public FitnessCenter? FitnessCenter { get; set; }
    }
} 