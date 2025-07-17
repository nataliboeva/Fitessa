using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Fitessa.Data;
using Fitessa.Data.Entities;
using System.Threading.Tasks;
using System.Linq;

namespace Fitessa.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminSubscriptionPlansController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AdminSubscriptionPlansController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var plans = _context.SubscriptionPlans.ToList();
            return View(plans);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubscriptionPlan plan)
        {
            System.Diagnostics.Debug.WriteLine("Create POST called");
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Model validation failed. Please check all fields.");
                return View(plan);
            }
            _context.SubscriptionPlans.Add(plan);
            await _context.SaveChangesAsync();
            TempData["PlanCreated"] = $"Plan '{plan.Name}' created successfully.";
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var plan = _context.SubscriptionPlans.Find(id);
            if (plan == null) return NotFound();
            return View(plan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SubscriptionPlan plan)
        {
            System.Diagnostics.Debug.WriteLine("Edit POST called");
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Model validation failed. Please check all fields.");
                return View(plan);
            }
            var dbPlan = _context.SubscriptionPlans.Find(plan.Id);
            if (dbPlan == null)
                return NotFound();
            dbPlan.Name = plan.Name;
            dbPlan.Price = plan.Price;
            dbPlan.DurationDays = plan.DurationDays;
            dbPlan.IsActive = plan.IsActive;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Database error: {ex.Message}");
                return View(plan);
            }
            TempData["PlanUpdated"] = $"Plan '{plan.Name}' updated successfully.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var plan = _context.SubscriptionPlans.Find(id);
            if (plan != null)
            {
                _context.SubscriptionPlans.Remove(plan);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
} 