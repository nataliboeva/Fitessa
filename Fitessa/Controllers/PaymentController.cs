using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Fitessa.Data.Entities;
using Fitessa.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Fitessa.Web.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PaymentController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var plans = await _context.SubscriptionPlans.Where(p => p.IsActive).ToListAsync();
            var userSubscriptions = await _context.UserSubscriptions
                .Where(us => us.UserId == user.Id)
                .Include(us => us.SubscriptionPlan)
                .OrderByDescending(us => us.StartDate)
                .ToListAsync();

            ViewBag.UserSubscriptions = userSubscriptions;
            return View(plans);
        }

        public async Task<IActionResult> Checkout(int planId)
        {
            var plan = await _context.SubscriptionPlans.FindAsync(planId);
            if (plan == null || !plan.IsActive)
            {
                return NotFound();
            }

            return View(plan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessPayment(int planId)
        {
            var user = await _userManager.GetUserAsync(User);
            var plan = await _context.SubscriptionPlans.FindAsync(planId);

            if (plan == null || !plan.IsActive)
            {
                return NotFound();
            }

            var userSubscription = new UserSubscription
            {
                UserId = user.Id,
                SubscriptionPlanId = planId,
                StartDate = System.DateTime.UtcNow,
                EndDate = System.DateTime.UtcNow.AddDays(30),
                Status = "Active",
                RenewalType = "Manual",
                PaymentId = "manual-purchase",
                IsActive = true,
                User = user,
                SubscriptionPlan = plan
            };

            _context.UserSubscriptions.Add(userSubscription);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Successfully subscribed to {plan.Name} plan!";
            return RedirectToAction("Success");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Subscribe(int planId)
        {
            return RedirectToAction("Checkout", new { planId = planId });
        }

        public IActionResult Success()
        {
            return View();
        }
    }
} 