using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Fitessa.Services.Interfaces;
using Fitessa.Data.Entities;
using DinkToPdf;
using DinkToPdf.Contracts;

namespace Fitessa.Web.Controllers
{
    [Authorize]
    public class WorkoutProgramsController : Controller
    {
        private readonly IWorkoutProgramService _service;
        private readonly IExerciseService _exerciseService;
        private readonly IConverter _pdfConverter;
        public WorkoutProgramsController(IWorkoutProgramService service, IExerciseService exerciseService, IConverter pdfConverter)
        {
            _service = service;
            _exerciseService = exerciseService;
            _pdfConverter = pdfConverter;
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
        public IActionResult ExportToPdf(int id)
        {
            var program = _service.GetById(id);
            if (program == null) return NotFound();
            var html = $@"<h1>{program.Name}</h1><p>{program.Description}</p><p><b>Difficulty:</b> {program.Difficulty}</p><p><b>Duration:</b> {program.DurationDays} days</p>";
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = { PaperSize = PaperKind.A4, Orientation = Orientation.Portrait },
                Objects = { new ObjectSettings { HtmlContent = html } }
            };
            var pdf = _pdfConverter.Convert(doc);
            return File(pdf, "application/pdf", $"WorkoutProgram_{program.Name}.pdf");
        }
    }
} 