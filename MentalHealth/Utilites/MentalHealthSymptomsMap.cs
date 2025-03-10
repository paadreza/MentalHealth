using CsvHelper.Configuration;
using MentalHealth.Models;
namespace MentalHealth.Utilites
{

    
        public class MentalHealthSymptomsMap : ClassMap<MentalHealthSymptoms>
        {
            public MentalHealthSymptomsMap()
            {
                Map(m => m.Anxiety).Index(0);
                Map(m => m.Depression).Index(1);
                Map(m => m.SleepIssues).Index(2);
                Map(m => m.MoodSwings).Index(3);
                Map(m => m.ConcentrationIssues).Index(4);
                Map(m => m.Fatigue).Index(5);
                Map(m => m.SocialWithdrawal).Index(6);
                Map(m => m.Irritability).Index(7);
                Map(m => m.AppetiteChanges).Index(8);
                Map(m => m.PhysicalSymptoms).Index(9);
                Map(m => m.TraumaHistory).Index(10);
                Map(m => m.ThoughtPatterns).Index(11);
                Map(m => m.SubstanceUse).Index(12);
                Map(m => m.Paranoia).Index(13);
                Map(m => m.Hallucinations).Index(14);
                Map(m => m.EmotionalChanges).Index(15);
                Map(m => m.Diagnosis).Index(16);
            }
        }
    
}
