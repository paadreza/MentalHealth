namespace MentalHealth.Models
{
    public class TrainingDataModel
    {
        public int Id { get; set; }
        public string Source { get; set; }
        public DateTime ImportDate { get; set; }
        public int RecordCount { get; set; }
        public string ValidationAccuracy { get; set; }
    }
}
