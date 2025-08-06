using System.Collections.Generic;

namespace Fitessa.Data.Entities
{
    public class City
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int CountryId { get; set; }
        public required Country Country { get; set; }
        public ICollection<FitnessCenter> FitnessCenters { get; set; } = new List<FitnessCenter>();
    }
} 