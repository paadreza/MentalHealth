using MentalHealth.Models;

namespace MentalHealth.Utilites
{
    
    
        public static class TrainingDataGenerator
        {
            public static IEnumerable<MentalHealthSymptoms> GenerateTrainingData()
            {
                var random = new Random();
                var diagnoses = new[]
                {
                "Major Depression",
                "Generalized Anxiety Disorder",
                "Bipolar Disorder",
                "Schizophrenia",
                "PTSD"
            };

                var trainingData = new List<MentalHealthSymptoms>();

                foreach (var diagnosis in diagnoses)
                {
                    for (int i = 0; i < 100; i++)
                    {
                        var symptoms = new MentalHealthSymptoms
                        {
                            Depression = GenerateSymptomValue(random, diagnosis == "Major Depression" ? 7 : 4),
                            Anxiety = GenerateSymptomValue(random, diagnosis == "Generalized Anxiety Disorder" ? 8 : 5),
                            SleepIssues = GenerateSymptomValue(random, diagnosis == "Major Depression" ? 6 : 4),
                            Fatigue = GenerateSymptomValue(random, diagnosis == "Major Depression" ? 7 : 3),
                            ConcentrationIssues = GenerateSymptomValue(random, diagnosis == "Schizophrenia" ? 8 : 4),
                            Irritability = GenerateSymptomValue(random, diagnosis == "Bipolar Disorder" ? 8 : 4),
                            SocialWithdrawal = GenerateSymptomValue(random, diagnosis == "Major Depression" ? 7 : 4),
                            EmotionalChanges = GenerateSymptomValue(random, diagnosis == "Bipolar Disorder" ? 9 : 4),
                            PhysicalSymptoms = GenerateSymptomValue(random, diagnosis == "Generalized Anxiety Disorder" ? 6 : 3),
                            ThoughtPatterns = GenerateSymptomValue(random, diagnosis == "Schizophrenia" ? 9 : 4),
                            Paranoia = GenerateSymptomValue(random, diagnosis == "Schizophrenia" ? 8 : 2),
                            Hallucinations = GenerateSymptomValue(random, diagnosis == "Schizophrenia" ? 7 : 1),
                            SubstanceUse = GenerateSymptomValue(random, 3),
                            TraumaHistory = GenerateSymptomValue(random, diagnosis == "PTSD" ? 9 : 3),
                            Diagnosis = diagnosis,
                            CreatedAt = DateTime.Now.AddDays(-random.Next(0, 30))
                        };
                        trainingData.Add(symptoms);
                    }
                }

                return trainingData;
            }

            private static float GenerateSymptomValue(Random random, float baseLine)
            {
                return Math.Min(10, Math.Max(0, (float)(baseLine + random.NextDouble() * 2 - 1)));
            }
        }
    
}
