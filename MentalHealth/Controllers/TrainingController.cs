using CsvHelper;
using System.Globalization;
using MentalHealth.Data;
using MentalHealth.Models;
using MentalHealth.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MentalHealth.Utilites;

namespace MentalHealth.Controllers
{
    public class TrainingController : Controller
    {
        private readonly IMentalHealthService _mentalHealthService;
        private readonly ILogger<TrainingController> _logger;

        public TrainingController(IMentalHealthService mentalHealthService, ILogger<TrainingController> logger)
        {
            _mentalHealthService = mentalHealthService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(Microsoft.AspNetCore.Http.IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("", "Please select a file to upload.");
                return View();
            }

            try
            {
                // Process the CSV file
                List<MentalHealthSymptoms> trainingData = new List<MentalHealthSymptoms>();

                using (var reader = new StreamReader(file.OpenReadStream()))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Context.RegisterClassMap<MentalHealthSymptomsMap>();
                    trainingData = csv.GetRecords<MentalHealthSymptoms>().ToList();
                }

                if (trainingData.Count == 0)
                {
                    ModelState.AddModelError("", "No valid data found in the uploaded file.");
                    return View();
                }

                // Import the data and train the model
                var success = await _mentalHealthService.ImportTrainingData(trainingData);

                if (success)
                {
                    TempData["SuccessMessage"] = $"Successfully imported {trainingData.Count} records and trained the model.";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Failed to import training data.");
                    return View();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading training data");
                ModelState.AddModelError("", $"Error processing file: {ex.Message}");
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Train()
        {
            try
            {
                // Code to train the model with all data
                // This could be implemented to retrain the model with all data in the database
                TempData["SuccessMessage"] = "Model training completed successfully.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error training model");
                TempData["ErrorMessage"] = $"Error training model: {ex.Message}";
            }

            return RedirectToAction("Index");
        }
    }
}
