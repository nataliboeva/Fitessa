using System.Collections.Generic;
using System.Linq;
using Fitessa.Data;
using Fitessa.Data.Entities;
using Fitessa.Services.Interfaces;

namespace Fitessa.Services.Services
{
    public class MealPlanService : IMealPlanService
    {
        private readonly ApplicationDbContext _context;
        public MealPlanService(ApplicationDbContext context)
        {
            _context = context;
        }
        public IEnumerable<MealPlan> GetByUser(string userId)
        {
            return _context.MealPlans.Where(m => m.UserId == userId).ToList();
        }
        public MealPlan GetById(int id)
        {
            return _context.MealPlans.FirstOrDefault(m => m.Id == id);
        }
        public void Create(MealPlan plan)
        {
            _context.MealPlans.Add(plan);
            _context.SaveChanges();
        }
        public void Update(MealPlan plan)
        {
            _context.MealPlans.Update(plan);
            _context.SaveChanges();
        }
        public void Delete(int id)
        {
            var plan = _context.MealPlans.Find(id);
            if (plan != null)
            {
                _context.MealPlans.Remove(plan);
                _context.SaveChanges();
            }
        }
        public IEnumerable<MealPlan> GetRecommended(string goal)
        {
            if (string.IsNullOrEmpty(goal))
                return _context.MealPlans.Take(1).ToList();
            return _context.MealPlans
                .Where(mp => mp.Name.Contains(goal) || mp.Description.Contains(goal))
                .Take(2)
                .ToList();
        }
    }
} 