using System;
using System.Linq;

namespace Images04
{
    public static class StatisticsCalculator
    {
        public static void CalculateStatistics(float[] vector, out float mean, out float variance, out float skewness, out float kurtosis, out float energy, out float entropy)
        {
            float NM = vector.Sum();

            float[] pi = vector;
            for (int i = 0; i < vector.Length; i++)
            {
                pi[i] = pi[i] / NM;
            }

            mean = 0;
            for (int i = 0; i < vector.Length; i++)
            {
                mean += i * pi[i];
            }

            variance = 0;
            for (int i = 0; i < vector.Length; i++)
            {
                variance += (float)Math.Pow((i - mean), 2) * pi[i];
            }

            skewness = 0;
            for (int i = 0; i < vector.Length; i++)
            {
                skewness += (float)Math.Pow((i - mean), 2) * pi[i];
            }

            skewness = (float)(skewness * Math.Pow(Math.Sqrt(variance), -3));

            kurtosis = 0;
            for (int i = 0; i < vector.Length; i++)
            {
                kurtosis += (float)Math.Pow(i - mean, 4) * pi[i] - 3;
            }

            kurtosis = (float)(kurtosis * Math.Pow(Math.Sqrt(variance), -4));

            energy = 0;
            for (int i = 0; i < vector.Length; i++)
            {
                energy += (float)Math.Pow(pi[i], 2);
            }

            entropy = 0;
            for (int i = 0; i < vector.Length; i++)
            {
                entropy = -pi[i] * (float)Math.Log(pi[i], 2);
            }

            entropy = float.IsNaN(entropy) ? 0 : entropy;
        }
    }
}
