using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Fitessa.Data;
using Fitessa.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Fitessa.Controllers
{
    [Authorize]
    public class SubscriptionPlansController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SubscriptionPlansController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            var plans = _context.SubscriptionPlans.Where(p => p.IsActive).ToList();
            return View(plans);
        }

        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            var plan = _context.SubscriptionPlans.Find(id);
            if (plan == null || !plan.IsActive) return NotFound();
            return View(plan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Purchase(int id)
        {
            var plan = _context.SubscriptionPlans.Find(id);
            if (plan == null || !plan.IsActive) return NotFound();
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();
            var userSub = _context.UserSubscriptions.FirstOrDefault(us => us.UserId == user.Id);
            if (userSub == null)
            {
                userSub = new UserSubscription
                {
                    UserId = user.Id,
                    SubscriptionPlanId = plan.Id,
                    StartDate = System.DateTime.UtcNow,
                    EndDate = System.DateTime.UtcNow.AddDays(plan.DurationDays),
                    Status = "Active",
                    RenewalType = "Manual"
                };
                _context.UserSubscriptions.Add(userSub);
            }
            else
            {
                userSub.SubscriptionPlanId = plan.Id;
                userSub.StartDate = System.DateTime.UtcNow;
                userSub.EndDate = System.DateTime.UtcNow.AddDays(plan.DurationDays);
                userSub.Status = "Active";
            }
            await _context.SaveChangesAsync();
            TempData["SubscriptionPurchased"] = plan.Name;
            return RedirectToAction("Index");
        }
    }
}
