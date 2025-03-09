namespace MentalHealth.Models
{
    public class ModelMetrics
    {
        public double MacroAccuracy { get; set; }
        public double MicroAccuracy { get; set; }
        public double LogLoss { get; set; }
        public double LogLossReduction { get; set; }
        public Dictionary<string, PerClassMetrics> PerClassMetrics { get; set; } = new Dictionary<string, PerClassMetrics>();
    }

    public class PerClassMetrics
    {
        public double Precision { get; set; }
        public double Recall { get; set; }
        public double F1Score { get; set; }
        public int ConfusionMatrix { get; set; }
    }
}
