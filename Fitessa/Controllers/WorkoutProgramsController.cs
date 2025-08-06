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

        [HttpGet]
        public IActionResult Filter(string difficulty, string duration)
        {
            var programs = _service.GetAll().AsQueryable();

            if (!string.IsNullOrWhiteSpace(difficulty))
            {
                programs = programs.Where(p => p.Difficulty == difficulty);
            }
            if (!string.IsNullOrWhiteSpace(duration))
            {
                if (int.TryParse(duration, out int durationDays))
                {
                    programs = programs.Where(p => p.DurationDays <= durationDays);
                }
            }

            var filteredPrograms = programs.ToList();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_WorkoutProgramsList", filteredPrograms);
            }

            return View("Index", filteredPrograms);
        }

        public IActionResult Details(int id)
        {
            var program = _service.GetById(id);
            if (program == null) return NotFound();
            ViewBag.Exercises = _exerciseService.GetAll();
            return View(program);
        }

        [HttpGet]
        public IActionResult Export(int id)
        {
            var program = _service.GetById(id);
            if (program == null) return NotFound();

            var exercises = _exerciseService.GetAll();
            var programExercises = exercises.Where(e => e.WorkoutProgramExercises.Any(wpe => wpe.WorkoutProgramId == id));

            var htmlContent = GenerateWorkoutProgramHtml(program, programExercises);
            var pdfBytes = ConvertHtmlToPdf(htmlContent);

            return File(pdfBytes, "application/pdf", $"workout-program-{id}.pdf");
        }

        private string GenerateWorkoutProgramHtml(WorkoutProgram program, IEnumerable<Exercise> exercises)
        {
            return $@"
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; margin: 20px; }}
                        .header {{ text-align: center; margin-bottom: 30px; }}
                        .program-info {{ margin-bottom: 20px; }}
                        .exercise {{ margin-bottom: 15px; padding: 10px; border: 1px solid #ddd; }}
                    </style>
                </head>
                <body>
                    <div class='header'>
                        <h1>{program.Name}</h1>
                        <p>Difficulty: {program.Difficulty} | Duration: {program.DurationDays} days</p>
                    </div>
                    <div class='program-info'>
                        <p><strong>Description:</strong> {program.Description}</p>
                    </div>
                    <h2>Exercises</h2>
                    {string.Join("", exercises.Select(e => $@"
                        <div class='exercise'>
                            <h3>{e.Name}</h3>
                            <p><strong>Muscle Group:</strong> {e.MuscleGroup}</p>
                            <p><strong>Difficulty:</strong> {e.DifficultyLevel}</p>
                            <p><strong>Description:</strong> {e.Description}</p>
                        </div>
                    "))}
                </body>
                </html>
            ";
        }

        private byte[] ConvertHtmlToPdf(string htmlContent)
        {
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings() { Top = 10, Bottom = 10, Left = 10, Right = 10 }
            };

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = htmlContent,
                WebSettings = { DefaultEncoding = "utf-8" }
            };

            var document = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            return _pdfConverter.Convert(document);
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
            var program = _service.GetById(id);
            if (program != null)
            {
                _service.Delete(id);
            }
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