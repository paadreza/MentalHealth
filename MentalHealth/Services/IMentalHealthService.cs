using MentalHealth.Models;

namespace MentalHealth.Services
{
    public interface IMentalHealthService
    {
        Task<MentalHealthPrediction> PredictDiagnosis(MentalHealthSymptoms symptoms);
        Task<ModelMetrics> EvaluateModel(IEnumerable<MentalHealthSymptoms> testData);
        Task TrainModel(IEnumerable<MentalHealthSymptoms> trainingData);
        Task<bool> ImportTrainingData(IEnumerable<MentalHealthSymptoms> data);
        IEnumerable<string> GetAvailableDiagnoses();
    }
}
