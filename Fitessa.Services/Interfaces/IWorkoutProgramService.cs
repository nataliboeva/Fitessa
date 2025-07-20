using System.Collections.Generic;
using Fitessa.Data.Entities;

namespace Fitessa.Services.Interfaces
{
    public interface IWorkoutProgramService
    {
        IEnumerable<WorkoutProgram> GetAll();
        WorkoutProgram GetById(int id);
        void Create(WorkoutProgram program);
        void Update(WorkoutProgram program);
        void Delete(int id);
        void AssignExercise(int programId, int exerciseId, int orderIndex, int reps, int sets);
        void RemoveExercise(int programId, int exerciseId);
    }
} 