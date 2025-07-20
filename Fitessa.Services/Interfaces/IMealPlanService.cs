using System.Collections.Generic;
using Fitessa.Data.Entities;

namespace Fitessa.Services.Interfaces
{
    public interface IMealPlanService
    {
        IEnumerable<MealPlan> GetByUser(string userId);
        MealPlan GetById(int id);
        void Create(MealPlan plan);
        void Update(MealPlan plan);
        void Delete(int id);
        IEnumerable<MealPlan> GetRecommended(string goal);
    }
} 