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
    }
}
