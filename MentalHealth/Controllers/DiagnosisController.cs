using MentalHealth.Models;
using MentalHealth.Services;
using Microsoft.AspNetCore.Mvc;

namespace MentalHealth.Controllers
{
    // Controllers/DiagnosisController.cs
    public class DiagnosisController : Controller
    {
        private readonly IMentalHealthService _mentalHealthService;
        private readonly ILogger<DiagnosisController> _logger;

        public DiagnosisController(IMentalHealthService mentalHealthService, ILogger<DiagnosisController> logger)
        {
            _mentalHealthService = mentalHealthService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var model = new MentalHealthSymptoms();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Diagnose(MentalHealthSymptoms symptoms)
        {
            try
            {
                // اطمینان حاصل کنید که همه فیلدها مقداردهی شده‌اند
                if (symptoms.TraumaHistory <= 0) symptoms.TraumaHistory = 0;
                if (symptoms.ThoughtPatterns <= 0) symptoms.ThoughtPatterns = 0;
                if (symptoms.SubstanceUse <= 0) symptoms.SubstanceUse = 0;
                if (symptoms.Paranoia <= 0) symptoms.Paranoia = 0;
                if (symptoms.Hallucinations <= 0) symptoms.Hallucinations = 0;
                if (symptoms.EmotionalChanges <= 0) symptoms.EmotionalChanges = 0;

                // فیلد Diagnosis را با یک مقدار موقت پر می‌کنیم
                symptoms.Diagnosis = "Unknown";

                if (!ModelState.IsValid)
                {
                    return View("Index", symptoms);
                }

                var result = await _mentalHealthService.PredictDiagnosis(symptoms);

                _logger.LogInformation($"Diagnosis prediction made: {result.Diagnosis} with probability {result.Probability}");

                return View("Result", result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during diagnosis");
                ViewBag.ErrorMessage = $"خطا در تشخیص: {ex.Message}";
                ModelState.AddModelError("", "An error occurred during diagnosis. Please try again.");
                return View("Index", symptoms);
            }
        }
    }
}
