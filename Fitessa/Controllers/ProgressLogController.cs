using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Fitessa.Data.Entities;
using Fitessa.Services.Interfaces;
using System.Threading.Tasks;

namespace Fitessa.Controllers
{
    [Authorize]
    public class ProgressLogController : Controller
    {
        private readonly IMeasurementLogService _service;
        private readonly UserManager<ApplicationUser> _userManager;
        public ProgressLogController(IMeasurementLogService service, UserManager<ApplicationUser> userManager)
        {
            _service = service;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var logs = _service.GetByUser(user.Id);
            return View(logs);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MeasurementLog log)
        {
            var user = await _userManager.GetUserAsync(User);
            if (!ModelState.IsValid) return View(log);
            log.UserId = user.Id;
            log.LoggedAt = System.DateTime.UtcNow;
            _service.Create(log);
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            var log = _service.GetById(id);
            if (log == null) return NotFound();
            return View(log);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(MeasurementLog log)
        {
            if (!ModelState.IsValid) return View(log);
            _service.Update(log);
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            var log = _service.GetById(id);
            if (log == null) return NotFound();
            return View(log);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _service.Delete(id);
            return RedirectToAction("Index");
        }
    }
} 