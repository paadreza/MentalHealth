// Utils/ConfusionMatrixExtensions.cs
using Microsoft.ML.Data;
using Microsoft.ML;
using MentalHealth.Models;
using MentalHealth.Utils;
namespace MentalHealth.Utils
{
    public static class ConfusionMatrixExtensions
    {
        public static float GetRowSum(this ConfusionMatrix matrix, int row)
        {
            float sum = 0;
            var counts = matrix.Counts;
            for (int i = 0; i < counts[row].Count; i++)
            {
                sum += (float)counts[row][i];
            }
            return sum;
        }

        public static float GetColumnSum(this ConfusionMatrix matrix, int col)
        {
            float sum = 0;
            var counts = matrix.Counts;
            for (int i = 0; i < counts.Count; i++)
            {
                sum += (float)counts[i][col];
            }
            return sum;
        }

        public static float GetCount(this ConfusionMatrix matrix, int row, int col)
        {
            return (float)matrix.Counts[row][col];
        }
    }
}
