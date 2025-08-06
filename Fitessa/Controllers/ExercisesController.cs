using Microsoft.AspNetCore.Mvc;
using Fitessa.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Fitessa.Web.Controllers
{
    public class ExercisesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExercisesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string search, string muscleGroup, string difficulty, int page = 1, int pageSize = 10)
        {
            var query = _context.Exercises.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(e => e.Name.Contains(search) || e.Description.Contains(search));
            }
            if (!string.IsNullOrWhiteSpace(muscleGroup))
            {
                query = query.Where(e => e.MuscleGroup == muscleGroup);
            }
            if (!string.IsNullOrWhiteSpace(difficulty))
            {
                query = query.Where(e => e.DifficultyLevel == difficulty);
            }

            int totalItems = await query.CountAsync();
            var exercises = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            ViewBag.MuscleGroups = _context.Exercises.Select(e => e.MuscleGroup).Distinct().ToList();
            ViewBag.Difficulties = _context.Exercises.Select(e => e.DifficultyLevel).Distinct().ToList();
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalItems = totalItems;

            return View(exercises);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string search, string muscleGroup, string difficulty)
        {
            var query = _context.Exercises.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(e => e.Name.Contains(search) || e.Description.Contains(search));
            }
            if (!string.IsNullOrWhiteSpace(muscleGroup))
            {
                query = query.Where(e => e.MuscleGroup == muscleGroup);
            }
            if (!string.IsNullOrWhiteSpace(difficulty))
            {
                query = query.Where(e => e.DifficultyLevel == difficulty);
            }

            var exercises = await query.ToListAsync();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_ExercisesList", exercises);
            }

            return View("Index", exercises);
        }

        public async Task<IActionResult> Details(int id)
        {
            var exercise = await _context.Exercises.FindAsync(id);
            if (exercise == null)
            {
                return NotFound();
            }
            return View(exercise);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Fitessa.Data.Entities.Exercise exercise)
        {
            if (!ModelState.IsValid) return View(exercise);
            _context.Exercises.Add(exercise);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(int id)
        {
            var exercise = await _context.Exercises.FindAsync(id);
            if (exercise == null) return NotFound();
            return View(exercise);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Fitessa.Data.Entities.Exercise exercise)
        {
            if (!ModelState.IsValid) return View(exercise);
            _context.Exercises.Update(exercise);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int id)
        {
            var exercise = await _context.Exercises.FindAsync(id);
            if (exercise == null) return NotFound();
            return View(exercise);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var exercise = await _context.Exercises.FindAsync(id);
            if (exercise != null)
            {
                _context.Exercises.Remove(exercise);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
} 