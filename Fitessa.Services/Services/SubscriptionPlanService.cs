using System.Collections.Generic;
using System.Linq;
using Fitessa.Data;
using Fitessa.Data.Entities;
using Fitessa.Services.Interfaces;

namespace Fitessa.Services.Services
{
    public class SubscriptionPlanService : ISubscriptionPlanService
    {
        private readonly ApplicationDbContext _context;

        public SubscriptionPlanService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<SubscriptionPlan> GetAllPlans()
        {
            return _context.SubscriptionPlans.Where(p => p.IsActive).ToList();
        }

        public IEnumerable<SubscriptionPlan> GetAll()
        {
            return _context.SubscriptionPlans.ToList();
        }

        public SubscriptionPlan GetById(int id)
        {
            return _context.SubscriptionPlans.FirstOrDefault(p => p.Id == id);
        }

        public void CreateUserSubscription(UserSubscription userSubscription)
        {
            _context.UserSubscriptions.Add(userSubscription);
            _context.SaveChanges();
        }
    }
}
