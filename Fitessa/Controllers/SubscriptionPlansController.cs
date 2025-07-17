using Microsoft.AspNetCore.Mvc;
using Fitessa.Services.Interfaces;

namespace Fitessa.Controllers
{
    public class SubscriptionPlansController : Controller
    {
        private readonly ISubscriptionPlanService _planService;

        public SubscriptionPlansController(ISubscriptionPlanService planService)
        {
            _planService = planService;
        }

        public IActionResult Index()
        {
            var plans = _planService.GetAllPlans();
            return View(plans);
        }
    }
}
