using System.Collections.Generic;
using System.Linq;
using Fitessa.Data;
using Fitessa.Data.Entities;
using Fitessa.Services.Interfaces;

namespace Fitessa.Services.Services
{
    public class WorkoutProgramService : IWorkoutProgramService
    {
        private readonly ApplicationDbContext _context;
        public WorkoutProgramService(ApplicationDbContext context)
        {
            _context = context;
        }
        public IEnumerable<WorkoutProgram> GetAll()
        {
            return _context.WorkoutPrograms.ToList();
        }
        public WorkoutProgram GetById(int id)
        {
            return _context.WorkoutPrograms
                .Where(wp => wp.Id == id)
                .FirstOrDefault();
        }
        public void Create(WorkoutProgram program)
        {
            _context.WorkoutPrograms.Add(program);
            _context.SaveChanges();
        }
        public void Update(WorkoutProgram program)
        {
            _context.WorkoutPrograms.Update(program);
            _context.SaveChanges();
        }
        public void Delete(int id)
        {
            var program = _context.WorkoutPrograms.Find(id);
            if (program != null)
            {
                _context.WorkoutPrograms.Remove(program);
                _context.SaveChanges();
            }
        }
        public void AssignExercise(int programId, int exerciseId, int orderIndex, int reps, int sets)
        {
            var assignment = new WorkoutProgramExercise
            {
                WorkoutProgramId = programId,
                ExerciseId = exerciseId,
                OrderIndex = orderIndex,
                Reps = reps,
                Sets = sets
            };
            _context.WorkoutProgramExercises.Add(assignment);
            _context.SaveChanges();
        }
        public void RemoveExercise(int programId, int exerciseId)
        {
            var assignment = _context.WorkoutProgramExercises.FirstOrDefault(x => x.WorkoutProgramId == programId && x.ExerciseId == exerciseId);
            if (assignment != null)
            {
                _context.WorkoutProgramExercises.Remove(assignment);
                _context.SaveChanges();
            }
        }
    }
} 