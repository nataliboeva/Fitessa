using System.Collections.Generic;
using Fitessa.Data.Entities;

namespace Fitessa.Services.Interfaces
{
    public interface ISubscriptionPlanService
    {
        IEnumerable<SubscriptionPlan> GetAllPlans();
    }
}
