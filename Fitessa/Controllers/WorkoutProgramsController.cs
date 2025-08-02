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
            
            var html = $@"
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; margin: 20px; }}
                        h1 {{ color: #333; }}
                        .info {{ margin: 10px 0; }}
                        .exercises {{ margin-top: 20px; }}
                    </style>
                </head>
                <body>
                    <h1>{program.Name}</h1>
                    <div class='info'>
                        <p><strong>Description:</strong> {program.Description}</p>
                        <p><strong>Difficulty:</strong> {program.Difficulty}</p>
                        <p><strong>Duration:</strong> {program.DurationDays} days</p>
                    </div>
                    <div class='exercises'>
                        <h2>Exercises</h2>
                        <p>This workout program includes various exercises to help you achieve your fitness goals.</p>
                    </div>
                </body>
                </html>";
            
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = { 
                    PaperSize = PaperKind.A4, 
                    Orientation = Orientation.Portrait,
                    Margins = new MarginSettings { Top = 20, Bottom = 20, Left = 20, Right = 20 }
                },
                Objects = { 
                    new ObjectSettings { 
                        HtmlContent = html,
                        WebSettings = { DefaultEncoding = "utf-8" }
                    } 
                }
            };
            
            try
            {
                var pdf = _pdfConverter.Convert(doc);
                return File(pdf, "application/pdf", $"WorkoutProgram_{program.Name.Replace(" ", "_")}.pdf");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Details", new { id = id });
            }
        }
    }
} 