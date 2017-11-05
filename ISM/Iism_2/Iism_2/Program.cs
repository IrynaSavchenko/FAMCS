using System;
using System.IO;

namespace Iism_2
{
    class Program
    {
        private const int N = 1000;
        //private const double PirsonLimit = 1073.643; //ХИ2ОБР(0,05; 999)
        private const double PirsonLimit = 54.57222776; //ХИ2ОБР(0,05; 39)
        private static int[] pirsonCorrectArray = new int[2];
        private const int NumberOfPirsonIntervals = 40;

        static void Main(string[] args)
        {
            string[] distrNamesArr = new string[2]
            {
                "---------------------POISSON---------------------",
                "---------------------GEOMETRIC---------------------"
            };

            BaseDistribuionRule[] distrArr;

            using (System.IO.StreamWriter file = new StreamWriter("output.txt"))
            {
                for (int i = 0; i < N; i++)
                {
                    distrArr = new BaseDistribuionRule[2]
                    {
                        new PoissonDistributionRule(0.2, N),
                        new GeometricDistributionRule(0.4, N)
                    };
                    int j = 0;
                    foreach (BaseDistribuionRule distribution in distrArr)
                    {
                        file.WriteLine(distrNamesArr[j]);
                        PerformOperations(distribution, j++, file);
                    }
                    file.WriteLine();
                    file.WriteLine();
                }

                file.WriteLine("===============================================");
                file.WriteLine($"Pirson correct %: poisson: {pirsonCorrectArray[0] / 10.0}, " +
                                                   $"geometric: {pirsonCorrectArray[1] / 10.0}");
            }
            Console.WriteLine("Completed!");
            Console.ReadLine();
        }

        private static void PerformOperations(BaseDistribuionRule distribution, int distrIndex, StreamWriter file)
        {
            file.WriteLine(distribution.TrueMathExpectation == null ?
                "No math expectation" :
                $"Math exp: calc = {distribution.MathExpectation}; real = {distribution.TrueMathExpectation}");

            file.WriteLine(distribution.TrueDispersion == null ?
                "No dispersion" :
                $"Dispersion exp: calc = {distribution.Dispersion}; real = {distribution.TrueDispersion}");

            bool isCorrectByPirson = IsCorrectByPirson(distribution);
            file.WriteLine($"Pirson correct: {isCorrectByPirson}");
            if (isCorrectByPirson)
            {
                pirsonCorrectArray[distrIndex]++;
            }
        }

        private static bool IsCorrectByPirson(BaseDistribuionRule distribution)
        {
            double[] values = distribution.ValuesArr;
            Array.Sort(values);

            //int K = 31;
            //double step = (values[N - 1] - values[0]) / K;

            //int[] frequences = new int[K];

            //for(int i = 0; i < N; i++)
            //{
            //    if (values[i] != values[0])
            //        break;
            //    frequences[0]++;
            //}

            //double prev = values[0];
            //for (int i = 0; i < K; i++)
            //{
            //    double next = prev + step;
            //    for (int j = 0; j < N; j++)
            //    {
            //        if (values[j] > prev && values[j] <= next)
            //        {
            //            frequences[i]++;
            //        }
            //    }
            //    prev += step;
            //}

            //int c = 0;
            //for (int i = 0; i < K; i++)
            //{
            //    c += frequences[i];
            //}

            //double[] p = new double[K];
            //prev = values[0];
            //for (int i = 1; i <= K; i++)
            //{
            //    double current = prev + step;
            //    p[i - 1] = distribution.CalculateDistributionFunction(current) -
            //        distribution.CalculateDistributionFunction(prev);
            //    prev += step;
            //}

            //double hi2 = 0.0;
            //for (int i = 0; i < K; i++)
            //{
            //    if(p[i] != 0)
            //        hi2 += Math.Pow(frequences[i] - N * p[i], 2) / (N * p[i]);
            //}

            //================== Gives 100% =============================
            int numberOfValuesInInterval = N / NumberOfPirsonIntervals;
            double[] bounds = GetArrayOfBounds(values, numberOfValuesInInterval);

            double[] p = new double[NumberOfPirsonIntervals];
            double prev = values[0];
            for (int i = 1; i <= NumberOfPirsonIntervals; i++)
            {
                double current = bounds[i - 1];
                p[i - 1] = distribution.CalculateDistributionFunction(current) -
                    distribution.CalculateDistributionFunction(prev);
                prev = current;
            }

            double hi2 = 0.0;
            for (int i = 0; i < NumberOfPirsonIntervals; i++)
            {
                if(p[i] != 0)
                    hi2 += Math.Pow(numberOfValuesInInterval - N * p[i], 2) / (N * p[i]);
            }
            return hi2 < PirsonLimit ? true : false;
        }

        private static double[] GetArrayOfBounds(double[] values, int numberOfValuesInInterval)
        {
            double[] bounds = new double[NumberOfPirsonIntervals];

            for (int i = numberOfValuesInInterval, j = 0; j < NumberOfPirsonIntervals; j++, i += numberOfValuesInInterval)
            {
                bounds[j] = values[i - 1];
            }
            return bounds;
        }
    }
}
