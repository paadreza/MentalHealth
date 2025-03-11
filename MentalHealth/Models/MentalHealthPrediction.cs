using Microsoft.ML.Data;

namespace MentalHealth.Models
{
    public class MentalHealthPrediction
    {
        [ColumnName("PredictedLabel")]
        public string Diagnosis { get; set; }

        public float Probability { get; set; }

        public List<string> Recommendations { get; set; } = new List<string>();

        public DateTime PredictionDate { get; set; } = DateTime.Now;

        public Dictionary<string, float> SymptomContributions { get; set; } = new Dictionary<string, float>();
        
        // Store the original symptoms for visualization
        public MentalHealthSymptoms Symptoms { get; set; }
    }
}
