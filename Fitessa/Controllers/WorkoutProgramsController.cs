using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Fitessa.Services.Interfaces;
using Fitessa.Data.Entities;

namespace Fitessa.Controllers
{
    [Authorize]
    public class WorkoutProgramsController : Controller
    {
        private readonly IWorkoutProgramService _service;
        private readonly IExerciseService _exerciseService;
        public WorkoutProgramsController(IWorkoutProgramService service, IExerciseService exerciseService)
        {
            _service = service;
            _exerciseService = exerciseService;
        }
        public IActionResult Index()
        {
            var programs = _service.GetAll();
            return View(programs);
        }
        public IActionResult Details(int id)
        {
            var program = _service.GetById(id);
            if (program == null) return NotFound();
            ViewBag.Exercises = _exerciseService.GetAll();
            return View(program);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(WorkoutProgram program)
        {
            if (!ModelState.IsValid) return View(program);
            _service.Create(program);
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            var program = _service.GetById(id);
            if (program == null) return NotFound();
            return View(program);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(WorkoutProgram program)
        {
            if (!ModelState.IsValid) return View(program);
            _service.Update(program);
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            var program = _service.GetById(id);
            if (program == null) return NotFound();
            return View(program);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _service.Delete(id);
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AssignExercise(int programId, int exerciseId, int orderIndex, int reps, int sets)
        {
            _service.AssignExercise(programId, exerciseId, orderIndex, reps, sets);
            return RedirectToAction("Details", new { id = programId });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveExercise(int programId, int exerciseId)
        {
            _service.RemoveExercise(programId, exerciseId);
            return RedirectToAction("Details", new { id = programId });
        }
    }
} 