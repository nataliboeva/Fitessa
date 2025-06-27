using System.Collections.Generic;

namespace Fitessa.Models.Entities
{
    public class FitnessCenter
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int CityId { get; set; }
        public bool IsActive { get; set; }
        public City City { get; set; }
        public ICollection<WorkoutSession> WorkoutSessions { get; set; }
    }
} 