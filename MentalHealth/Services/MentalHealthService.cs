using Microsoft.ML;
using Microsoft.ML.Data;
using MentalHealth.Models;
using Microsoft.Extensions.Logging;

namespace MentalHealth.Services
{
    public class MentalHealthService : IMentalHealthService
    {
        private readonly ILogger<MentalHealthService> _logger;
        private readonly MLContext _mlContext;
        private ITransformer _trainedModel;
        private List<string> _availableDiagnoses = new List<string> { "Anxiety", "Depression", "Bipolar", "Schizophrenia", "Other" };

        public MentalHealthService(ILogger<MentalHealthService> logger)
        {
            _logger = logger;
            _mlContext = new MLContext(seed: 1);

            // Load or train initial model
            InitializeModel();
        }

        private void InitializeModel()
        {
            try
            {
                // Generate some initial training data
                var trainingData = GenerateInitialTrainingData();

                // Train the model
                TrainModel(trainingData).Wait();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error initializing model");
            }
        }

        public async Task<MentalHealthPrediction> PredictDiagnosis(MentalHealthSymptoms symptoms)
        {
            try
            {
                _logger?.LogInformation("Starting simplified prediction");

                // Simple mock prediction based on symptoms
                string diagnosis = "Unknown";
                float probability = 0.0f;

                // Very simplified logic for demo
                if (symptoms.Anxiety > 7 && symptoms.Anxiety >= symptoms.Depression && symptoms.Anxiety >= symptoms.MoodSwings)
                {
                    diagnosis = "Anxiety";
                    probability = 0.7f + (symptoms.Anxiety - 7) * 0.05f;
                }
                else if (symptoms.Depression > 7 && symptoms.Depression >= symptoms.Anxiety && symptoms.Depression >= symptoms.MoodSwings)
                {
                    diagnosis = "Depression";
                    probability = 0.7f + (symptoms.Depression - 7) * 0.05f;
                }
                else if (symptoms.MoodSwings > 7)
                {
                    diagnosis = "Bipolar";
                    probability = 0.7f + (symptoms.MoodSwings - 7) * 0.05f;
                }
                else if ((symptoms.Hallucinations > 6 || symptoms.Paranoia > 6) &&
                         (symptoms.Hallucinations + symptoms.Paranoia >= symptoms.Anxiety + symptoms.Depression))
                {
                    diagnosis = "Schizophrenia";
                    probability = 0.7f + (Math.Max(symptoms.Hallucinations, symptoms.Paranoia) - 6) * 0.05f;
                }
                else
                {
                    // Find highest symptom
                    float maxAnxiety = symptoms.Anxiety;
                    float maxDepression = symptoms.Depression;
                    float maxBipolar = symptoms.MoodSwings;
                    float maxSchizophrenia = Math.Max(symptoms.Hallucinations, symptoms.Paranoia);

                    float maxValue = Math.Max(maxAnxiety,
                                   Math.Max(maxDepression,
                                   Math.Max(maxBipolar, maxSchizophrenia)));

                    if (maxValue == maxAnxiety)
                    {
                        diagnosis = "Anxiety";
                        probability = 0.5f + maxValue * 0.03f;
                    }
                    else if (maxValue == maxDepression)
                    {
                        diagnosis = "Depression";
                        probability = 0.5f + maxValue * 0.03f;
                    }
                    else if (maxValue == maxBipolar)
                    {
                        diagnosis = "Bipolar";
                        probability = 0.5f + maxValue * 0.03f;
                    }
                    else
                    {
                        diagnosis = "Schizophrenia";
                        probability = 0.5f + maxValue * 0.03f;
                    }
                }

                // Cap probability at 0.95
                probability = Math.Min(probability, 0.95f);

                _logger?.LogInformation($"Determined diagnosis: {diagnosis} with probability {probability}");

                // Ensure all properties are initialized
                if (float.IsNaN(symptoms.TraumaHistory)) symptoms.TraumaHistory = 0;
                if (float.IsNaN(symptoms.ThoughtPatterns)) symptoms.ThoughtPatterns = 0;
                if (float.IsNaN(symptoms.SubstanceUse)) symptoms.SubstanceUse = 0;
                if (float.IsNaN(symptoms.Paranoia)) symptoms.Paranoia = 0;
                if (float.IsNaN(symptoms.Hallucinations)) symptoms.Hallucinations = 0;
                if (float.IsNaN(symptoms.EmotionalChanges)) symptoms.EmotionalChanges = 0;

                var prediction = new MentalHealthPrediction
                {
                    Diagnosis = diagnosis,
                    Probability = probability,
                    Recommendations = GenerateRecommendations(diagnosis, symptoms),
                    SymptomContributions = new Dictionary<string, float>
            {
                { "Anxiety", symptoms.Anxiety / 10f },
                { "Depression", symptoms.Depression / 10f },
                { "Sleep Issues", symptoms.SleepIssues / 10f },
                { "Mood Swings", symptoms.MoodSwings / 10f },
                { "Concentration Issues", symptoms.ConcentrationIssues / 10f },
                { "Fatigue", symptoms.Fatigue / 10f },
                { "Social Withdrawal", symptoms.SocialWithdrawal / 10f },
                { "Irritability", symptoms.Irritability / 10f },
                { "Appetite Changes", symptoms.AppetiteChanges / 10f },
                { "Physical Symptoms", symptoms.PhysicalSymptoms / 10f },
                { "Trauma History", symptoms.TraumaHistory / 10f },
                { "Thought Patterns", symptoms.ThoughtPatterns / 10f },
                { "Substance Use", symptoms.SubstanceUse / 10f },
                { "Paranoia", symptoms.Paranoia / 10f },
                { "Hallucinations", symptoms.Hallucinations / 10f },
                { "Emotional Changes", symptoms.EmotionalChanges / 10f }
            }
                };

                _logger?.LogInformation("Prediction completed successfully");

                return prediction;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error making prediction: {Message}", ex.Message);
                throw;
            }
        }

        public async Task TrainModel(IEnumerable<MentalHealthSymptoms> trainingData)
        {
            try
            {
                // Convert training data to IDataView
                var trainingDataView = _mlContext.Data.LoadFromEnumerable(trainingData);

                // Define data preparation pipeline
                var pipeline = _mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "Label", inputColumnName: nameof(MentalHealthSymptoms.Diagnosis))
                    .Append(_mlContext.Transforms.Concatenate("Features",
                        nameof(MentalHealthSymptoms.Anxiety),
                        nameof(MentalHealthSymptoms.Depression),
                        nameof(MentalHealthSymptoms.SleepIssues),
                        nameof(MentalHealthSymptoms.MoodSwings),
                        nameof(MentalHealthSymptoms.ConcentrationIssues),
                        nameof(MentalHealthSymptoms.Fatigue),
                        nameof(MentalHealthSymptoms.SocialWithdrawal),
                        nameof(MentalHealthSymptoms.Irritability),
                        nameof(MentalHealthSymptoms.AppetiteChanges),
                        nameof(MentalHealthSymptoms.PhysicalSymptoms),
                        nameof(MentalHealthSymptoms.TraumaHistory),
                        nameof(MentalHealthSymptoms.ThoughtPatterns),
                        nameof(MentalHealthSymptoms.SubstanceUse),
                        nameof(MentalHealthSymptoms.Paranoia),
                        nameof(MentalHealthSymptoms.Hallucinations),
                        nameof(MentalHealthSymptoms.EmotionalChanges)))
                    .Append(_mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy("Label", "Features"))
                    .Append(_mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));
                // Train model
                _trainedModel = pipeline.Fit(trainingDataView);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error training model");
                throw;
            }
        }

        public async Task<ModelMetrics> EvaluateModel(IEnumerable<MentalHealthSymptoms> testData)
        {
            try
            {
                var testDataView = _mlContext.Data.LoadFromEnumerable(testData);
                var predictions = _trainedModel.Transform(testDataView);

                var metrics = _mlContext.MulticlassClassification.Evaluate(predictions);

                var modelMetrics = new ModelMetrics
                {
                    MacroAccuracy = metrics.MacroAccuracy,
                    MicroAccuracy = metrics.MicroAccuracy,
                    LogLoss = metrics.LogLoss,
                    LogLossReduction = metrics.LogLossReduction,
                    PerClassMetrics = new Dictionary<string, PerClassMetrics>()
                };

                // Calculate per-class metrics
                var matrix = metrics.ConfusionMatrix.Counts;
                var distinctLabels = testData.Select(x => x.Diagnosis).Distinct().ToList();

                for (int i = 0; i < distinctLabels.Count && i < matrix.Count; i++)
                {
                    var label = distinctLabels[i];

                    double tp = matrix[i][i];

                    double fp = 0.0;
                    for (int row = 0; row < matrix.Count; row++)
                    {
                        if (row != i) fp += matrix[row][i];
                    }

                    double fn = 0.0;
                    for (int col = 0; col < matrix[i].Count; col++)
                    {
                        if (col != i) fn += matrix[i][col];
                    }

                    double precision = tp / (tp + fp);
                    double recall = tp / (tp + fn);
                    double f1Score = 2 * (precision * recall) / (precision + recall);

                    modelMetrics.PerClassMetrics[label] = new PerClassMetrics
                    {
                        Precision = double.IsNaN(precision) ? 0 : precision,
                        Recall = double.IsNaN(recall) ? 0 : recall,
                        F1Score = double.IsNaN(f1Score) ? 0 : f1Score,
                        ConfusionMatrix = (int)tp
                    };
                }

                return modelMetrics;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error evaluating model");
                throw;
            }
        }

        public async Task<bool> ImportTrainingData(IEnumerable<MentalHealthSymptoms> data)
        {
            try
            {
                // Update available diagnoses
                foreach (var diagnosis in data.Select(d => d.Diagnosis).Distinct())
                {
                    if (!_availableDiagnoses.Contains(diagnosis))
                    {
                        _availableDiagnoses.Add(diagnosis);
                    }
                }

                // Train model with new data
                await TrainModel(data);
                return true;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error importing training data");
                return false;
            }
        }

        public IEnumerable<string> GetAvailableDiagnoses()
        {
            return _availableDiagnoses;
        }

        private IEnumerable<MentalHealthSymptoms> GenerateInitialTrainingData()
        {
            var trainingData = new List<MentalHealthSymptoms>();
            var random = new Random(42); // Fixed seed for reproducibility

            // Generate anxiety cases
            for (int i = 0; i < 100; i++)
            {
                trainingData.Add(new MentalHealthSymptoms
                {
                    Anxiety = random.Next(7, 11),
                    Depression = random.Next(0, 6),
                    SleepIssues = random.Next(5, 11),
                    MoodSwings = random.Next(3, 8),
                    ConcentrationIssues = random.Next(5, 9),
                    Fatigue = random.Next(3, 8),
                    SocialWithdrawal = random.Next(2, 7),
                    Irritability = random.Next(4, 9),
                    AppetiteChanges = random.Next(2, 7),
                    PhysicalSymptoms = random.Next(6, 11),
                    TraumaHistory = random.Next(3, 8),
                    ThoughtPatterns = random.Next(4, 9),
                    SubstanceUse = random.Next(0, 6),
                    Paranoia = random.Next(1, 5),
                    Hallucinations = random.Next(0, 3),
                    EmotionalChanges = random.Next(4, 9),
                    Diagnosis = "Anxiety"
                });
            }

            // Generate depression cases
            for (int i = 0; i < 100; i++)
            {
                trainingData.Add(new MentalHealthSymptoms
                {
                    Anxiety = random.Next(2, 7),
                    Depression = random.Next(7, 11),
                    SleepIssues = random.Next(6, 11),
                    MoodSwings = random.Next(4, 9),
                    ConcentrationIssues = random.Next(6, 11),
                    Fatigue = random.Next(7, 11),
                    SocialWithdrawal = random.Next(7, 11),
                    Irritability = random.Next(4, 9),
                    AppetiteChanges = random.Next(6, 11),
                    PhysicalSymptoms = random.Next(3, 8),
                    TraumaHistory = random.Next(4, 9),
                    ThoughtPatterns = random.Next(6, 10),
                    SubstanceUse = random.Next(2, 7),
                    Paranoia = random.Next(1, 6),
                    Hallucinations = random.Next(0, 4),
                    EmotionalChanges = random.Next(5, 10),
                    Diagnosis = "Depression"
                });
            }

            // Generate bipolar cases
            for (int i = 0; i < 100; i++)
            {
                trainingData.Add(new MentalHealthSymptoms
                {
                    Anxiety = random.Next(4, 9),
                    Depression = random.Next(5, 10),
                    SleepIssues = random.Next(6, 11),
                    MoodSwings = random.Next(8, 11),
                    ConcentrationIssues = random.Next(5, 10),
                    Fatigue = random.Next(5, 10),
                    SocialWithdrawal = random.Next(4, 9),
                    Irritability = random.Next(7, 11),
                    AppetiteChanges = random.Next(5, 10),
                    PhysicalSymptoms = random.Next(4, 9),
                    TraumaHistory = random.Next(3, 8),
                    ThoughtPatterns = random.Next(5, 10),
                    SubstanceUse = random.Next(3, 8),
                    Paranoia = random.Next(2, 7),
                    Hallucinations = random.Next(1, 6),
                    EmotionalChanges = random.Next(7, 11),
                    Diagnosis = "Bipolar"
                });
            }

            // Generate schizophrenia cases
            for (int i = 0; i < 100; i++)
            {
                trainingData.Add(new MentalHealthSymptoms
                {
                    Anxiety = random.Next(5, 10),
                    Depression = random.Next(5, 10),
                    SleepIssues = random.Next(6, 11),
                    MoodSwings = random.Next(5, 10),
                    ConcentrationIssues = random.Next(7, 11),
                    Fatigue = random.Next(5, 10),
                    SocialWithdrawal = random.Next(7, 11),
                    Irritability = random.Next(5, 10),
                    AppetiteChanges = random.Next(5, 10),
                    PhysicalSymptoms = random.Next(3, 8),
                    TraumaHistory = random.Next(4, 9),
                    ThoughtPatterns = random.Next(8, 11),
                    SubstanceUse = random.Next(3, 8),
                    Paranoia = random.Next(7, 11),
                    Hallucinations = random.Next(7, 11),
                    EmotionalChanges = random.Next(6, 11),
                    Diagnosis = "Schizophrenia"
                });
            }

            return trainingData;
        }

        private List<string> GenerateRecommendations(string diagnosis, MentalHealthSymptoms symptoms)
        {
            var recommendations = new List<string>();

            switch (diagnosis.ToLower())
            {
                case "anxiety":
                    recommendations.Add("Consider practicing relaxation techniques such as deep breathing or meditation");
                    recommendations.Add("Regular exercise can help reduce anxiety symptoms");
                    if (symptoms.SleepIssues > 5)
                        recommendations.Add("Establish a regular sleep schedule and bedtime routine");
                    if (symptoms.PhysicalSymptoms > 7)
                        recommendations.Add("Consider consulting with a healthcare provider about physical symptoms");
                    break;

                case "depression":
                    recommendations.Add("Maintain regular daily routines and activities");
                    recommendations.Add("Stay connected with supportive friends and family");
                    if (symptoms.SocialWithdrawal > 7)
                        recommendations.Add("Gradually increase social interactions, even if brief");
                    if (symptoms.Fatigue > 7)
                        recommendations.Add("Set small, achievable daily goals to build momentum");
                    break;

                case "bipolar":
                    recommendations.Add("Maintain a consistent daily schedule");
                    recommendations.Add("Track your mood changes and symptoms");
                    if (symptoms.MoodSwings > 7)
                        recommendations.Add("Learn to identify early warning signs of mood episodes");
                    if (symptoms.SleepIssues > 6)
                        recommendations.Add("Prioritize regular sleep patterns");
                    break;

                case "schizophrenia":
                    recommendations.Add("Maintain regular contact with a mental health professional");
                    recommendations.Add("Take medications as prescribed");
                    if (symptoms.Hallucinations > 5)
                        recommendations.Add("Develop coping strategies for hallucinations");
                    if (symptoms.Paranoia > 5)
                        recommendations.Add("Practice reality testing techniques");
                    if (symptoms.SocialWithdrawal > 6)
                        recommendations.Add("Participate in structured social activities");
                    break;

                default:
                    recommendations.Add("Consider scheduling an appointment with a mental health professional");
                    recommendations.Add("Keep track of your symptoms and their frequency");
                    break;
            }

            recommendations.Add("Consider seeking professional mental health support for a comprehensive evaluation");
            return recommendations;
        }
    }
}