using System;

namespace Fitessa.Models.Entities
{
    public class WorkoutSession
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int? WorkoutProgramId { get; set; }
        public string Title { get; set; }
        public DateTime ScheduledTime { get; set; }
        public bool IsCompleted { get; set; }
        public int? DurationMin { get; set; }
        public string Mood { get; set; }
        public int? FitnessCenterId { get; set; }
        public ApplicationUser User { get; set; }
        public WorkoutProgram WorkoutProgram { get; set; }
        public FitnessCenter FitnessCenter { get; set; }
    }
} 