using System.ComponentModel.DataAnnotations;
using Microsoft.ML.Data;

namespace MentalHealth.Models
{
    public class MentalHealthSymptoms
    {
        [LoadColumn(0)]
        public float Anxiety { get; set; }

        [LoadColumn(1)]
        public float Depression { get; set; }

        [LoadColumn(2)]
        public float SleepIssues { get; set; }

        [LoadColumn(3)]
        public float MoodSwings { get; set; }

        [LoadColumn(4)]
        public float ConcentrationIssues { get; set; }

        [LoadColumn(5)]
        public float Fatigue { get; set; }

        [LoadColumn(6)]
        public float SocialWithdrawal { get; set; }

        [LoadColumn(7)]
        public float Irritability { get; set; }

        [LoadColumn(8)]
        public float AppetiteChanges { get; set; }

        [LoadColumn(9)]
        public float PhysicalSymptoms { get; set; }

        [LoadColumn(10)]
        public float TraumaHistory { get; set; }

        [LoadColumn(11)]
        public float ThoughtPatterns { get; set; }

        [LoadColumn(12)]
        public float SubstanceUse { get; set; }

        [LoadColumn(13)]
        public float Paranoia { get; set; }

        [LoadColumn(14)]
        public float Hallucinations { get; set; }

        [LoadColumn(15)]
        public float EmotionalChanges { get; set; }

        [LoadColumn(16)]
        public string Diagnosis { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

}
