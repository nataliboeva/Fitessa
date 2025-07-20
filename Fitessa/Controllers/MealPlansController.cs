using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Fitessa.Data.Entities;
using Fitessa.Services.Interfaces;
using System.Threading.Tasks;
using DinkToPdf;
using DinkToPdf.Contracts;

namespace Fitessa.Web.Controllers
{
    [Authorize]
    public class MealPlansController : Controller
    {
        private readonly IMealPlanService _service;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConverter _pdfConverter;
        public MealPlansController(IMealPlanService service, UserManager<ApplicationUser> userManager, IConverter pdfConverter)
        {
            _service = service;
            _userManager = userManager;
            _pdfConverter = pdfConverter;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var plans = _service.GetByUser(user.Id);
            return View(plans);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MealPlan plan)
        {
            var user = await _userManager.GetUserAsync(User);
            if (!ModelState.IsValid) return View(plan);
            plan.UserId = user.Id;
            _service.Create(plan);
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            var plan = _service.GetById(id);
            if (plan == null) return NotFound();
            return View(plan);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(MealPlan plan)
        {
            if (!ModelState.IsValid) return View(plan);
            _service.Update(plan);
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            var plan = _service.GetById(id);
            if (plan == null) return NotFound();
            return View(plan);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _service.Delete(id);
            return RedirectToAction("Index");
        }
        public IActionResult ExportToPdf(int id)
        {
            var plan = _service.GetById(id);
            if (plan == null) return NotFound();
            var html = $@"<h1>{plan.Name}</h1><p>{plan.Description}</p>";
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = { PaperSize = PaperKind.A4, Orientation = Orientation.Portrait },
                Objects = { new ObjectSettings { HtmlContent = html } }
            };
            var pdf = _pdfConverter.Convert(doc);
            return File(pdf, "application/pdf", $"MealPlan_{plan.Name}.pdf");
        }
    }
} 