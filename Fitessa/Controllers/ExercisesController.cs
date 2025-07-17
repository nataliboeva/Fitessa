using Microsoft.AspNetCore.Mvc;
using Fitessa.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Fitessa.Controllers
{
    public class ExercisesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExercisesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string search, string muscleGroup, string difficulty)
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

            // For dropdowns
            ViewBag.MuscleGroups = _context.Exercises.Select(e => e.MuscleGroup).Distinct().ToList();
            ViewBag.Difficulties = _context.Exercises.Select(e => e.DifficultyLevel).Distinct().ToList();

            return View(exercises);
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
    }
} 