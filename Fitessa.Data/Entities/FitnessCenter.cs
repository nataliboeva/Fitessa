using System.Collections.Generic;

namespace Fitessa.Data.Entities
{
    public class FitnessCenter
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Address { get; set; }
        public int CityId { get; set; }
        public bool IsActive { get; set; }
        public required City City { get; set; }
        public ICollection<WorkoutSession> WorkoutSessions { get; set; } = new List<WorkoutSession>();
    }
} 