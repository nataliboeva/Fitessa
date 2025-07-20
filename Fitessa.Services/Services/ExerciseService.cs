using System.Collections.Generic;
using System.Linq;
using Fitessa.Data;
using Fitessa.Data.Entities;
using Fitessa.Services.Interfaces;

namespace Fitessa.Services.Services
{
    public class ExerciseService : IExerciseService
    {
        private readonly ApplicationDbContext _context;
        public ExerciseService(ApplicationDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Exercise> GetAll()
        {
            return _context.Exercises.ToList();
        }
    }
} 