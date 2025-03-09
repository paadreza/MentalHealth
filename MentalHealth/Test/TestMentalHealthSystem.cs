//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using MentalHealth.Services;
//using MentalHealth.Models;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//namespace MentalHealth.Test
//{
//    [TestClass]
//    public class MentalHealthSystemTests
//    {
//        private MentalHealthService _service;

//        [TestInitialize]
//        public void Setup()
//        {
//            _service = new MentalHealthService(null); // Pass null for logger or mock it
//        }

//        [TestMethod]
//        public async Task TestDiagnosis_Anxiety()
//        {
//            // Arrange
//            var symptoms = new MentalHealthSymptoms
//            {
//                Anxiety = 8,
//                Depression = 3,
//                SleepIssues = 7,
//                MoodSwings = 4,
//                ConcentrationIssues = 6,
//                Fatigue = 5,
//                SocialWithdrawal = 4,
//                Irritability = 6,
//                AppetiteChanges = 3,
//                PhysicalSymptoms = 7
//            };

//            // Act
//            var result = await _service.PredictDiagnosis(symptoms);

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.IsTrue(result.Probability > 0);
//        }

//        [TestMethod]
//        public async Task TestDiagnosis_Depression()
//        {
//            // Arrange
//            var symptoms = new MentalHealthSymptoms
//            {
//                Anxiety = 3,
//                Depression = 8,
//                SleepIssues = 7,
//                MoodSwings = 6,
//                ConcentrationIssues = 7,
//                Fatigue = 8,
//                SocialWithdrawal = 8,
//                Irritability = 5,
//                AppetiteChanges = 7,
//                PhysicalSymptoms = 4
//            };

//            // Act
//            var result = await _service.D(symptoms);

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.IsTrue(result.Probability > 0);
//        }

//        [TestMethod]
//        public async Task TestModelEvaluation()
//        {
//            // Arrange
//            var testData = new List<MentalHealthSymptoms>
//        {
//            new MentalHealthSymptoms
//            {
//                Anxiety = 8,
//                Depression = 3,
//                SleepIssues = 7,
//               // MoodSwings = 4,
//                ConcentrationIssues = 6,
//                Fatigue = 5,
//                SocialWithdrawal = 4,
//                Irritability = 6,
//               // AppetiteChanges = 3,
//                PhysicalSymptoms = 7,
//                Diagnosis = "Anxiety"
//            },
//            new MentalHealthSymptoms
//            {
//                Anxiety = 3,
//                Depression = 8,
//                SleepIssues = 7,
//               // MoodSwings = 6,
//                ConcentrationIssues = 7,
//                Fatigue = 8,
//                SocialWithdrawal = 8,
//                Irritability = 5,
//               // AppetiteChanges = 7,
//                PhysicalSymptoms = 4,
//                Diagnosis = "Depression"
//            }
//        };

//            // Act
//            var metrics = await _service.EvaluateModel(testData);

//            // Assert
//            Assert.IsNotNull(metrics);
//            Assert.IsTrue(metrics.MacroAccuracy >= 0);
//            Assert.IsTrue(metrics.MicroAccuracy >= 0);
//        }
//    } 
//}
