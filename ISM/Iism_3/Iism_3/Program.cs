using System;
using System.IO;

namespace Iism_3
{
    class Program
    {
        private const int N = 1000;
        private const double KolmogorovLimit = 1.36;
        private const double PirsonLimit = 1073.643; //ХИ2ОБР(0,05; 999)

        private static int[] kolmogorovCorrectArray = new int[3];
        private static int[] pirsonCorrectArray = new int[3];

        public static void Main(string[] args)
        {
            string[] distrNamesArr = new string[3]
            {
                "---------------------NORMAL---------------------",
                "---------------------CAUCHY---------------------",
                "---------------------LOGISTIC---------------------"
            };

            BaseDistribuionRule[] distrArr;

            using (StreamWriter file = new StreamWriter("output.txt"))
            {
                for (int i = 0; i < N; i++)
                {
                    distrArr = new BaseDistribuionRule[3]
                    {
                    new NormalDistributionRule(-1, 4, N),
                    new CauchyDistributionRule(-1, 1, N),
                    new LogisticDistributionRule(2, 3, N)
                    };
                    int j = 0;
                    foreach (BaseDistribuionRule distribution in distrArr)
                    {
                        //file.WriteLine(distrNamesArr[j]);
                        PerformOperations(distribution, j++, file);
                    }
                    //file.WriteLine();
                    //file.WriteLine();
                }

                file.WriteLine("===============================================");
                file.WriteLine($"Kolmogorov correct %: normal: {kolmogorovCorrectArray[0] / 10.0}, " +
                                                        $"cauchy: {kolmogorovCorrectArray[1] / 10.0}, " +
                                                        $"logistic: {kolmogorovCorrectArray[2] / 10.0}");

                file.WriteLine("===============================================");
                file.WriteLine($"Pirson correct %: normal: {pirsonCorrectArray[0] / 10.0}, " +
                                                        $"cauchy: {pirsonCorrectArray[1] / 10.0}, " +
                                                        $"logistic: {pirsonCorrectArray[2] / 10.0}");
            }
            Console.WriteLine("Completed!");
            Console.ReadLine();
        }

        private static void PerformOperations(BaseDistribuionRule distribution, int distrIndex, StreamWriter file)
        {
            //file.WriteLine(distribution.TrueMathExpectation == null ? 
            //    "No math expectation" : 
            //    $"Math exp: calc = {distribution.MathExpectation}; real = {distribution.TrueMathExpectation}");

            //file.WriteLine(distribution.TrueDispersion == null ?
            //    "No dispersion" :
            //    $"Dispersion exp: calc = {distribution.Dispersion}; real = {distribution.TrueDispersion}");

            bool isCorrectByKolmogorov = IsCorrectByKolmogorov(distribution);
            //file.WriteLine($"Kolmogorov correct: {isCorrectByKolmogorov}");
            if(isCorrectByKolmogorov)
            {
                kolmogorovCorrectArray[distrIndex]++;
            }

            bool isCorrectByPirson = IsCorrectByPirson(distribution);
            //file.WriteLine($"Pirson correct: {isCorrectByPirson}");
            if (isCorrectByPirson)
            {
                pirsonCorrectArray[distrIndex]++;
            }
        }

        private static bool IsCorrectByKolmogorov(BaseDistribuionRule distribution)
        {
            distribution.Sort();

            double[] dn = new double[N];
            for (int i = 0; i < N; i++)
            {
                double currentFunc = i / (double) N;
                double nextFunc = (i + 1) / (double) N;
                dn[i] = Math.Max(Math.Abs(currentFunc - distribution.F[i]),
                    Math.Abs(nextFunc - distribution.F[i]));
                if(dn[i] < 0 || dn[i] > 1)
                {
                    dn[i] = 0;
                }

            }
            double maxDn = dn[0];
            for (int i = 0; i < N; i++)
            {
                if (maxDn < dn[i]) maxDn = dn[i];
            }
            double stat = maxDn * Math.Sqrt(N);

            return stat < KolmogorovLimit ? true : false;
        }

        private static bool IsCorrectByPirson(BaseDistribuionRule distribution)
        {
            double[] values = distribution.ValuesArr;
            Array.Sort(values);

            int K = 31;
            //int moreK = 39;
            double step = (values[N - 1] - values[0]) / (double) K;

            double range = step * 4;

            int[] frequences = new int[K];
            for (int j = 0; j < N; j++)
                {
                    if (values[j] > values[N - 1] - range && values[j] <= values[N - 1])
                    {
                        frequences[K - 1]++;
                    }
                }

            for (int j = 0; j < N; j++)
            {
                if (values[j] > values[0] && values[j] <= values[0] + range)
                {
                    frequences[0]++;
                }
            }

            //int[] frequences = new int[K];
            //frequences[0] = 1;
            double prev = values[0];
            for(int i = 4; i < K - 4; i++)
            {
                double next = prev + step;
                for(int j = 0; j < N; j++)
                {
                    if(values[j] > prev && values[j] <= next)
                    {
                        frequences[i]++;
                    }
                }
                prev += step;
            }
            K = 25;
            double[] p = new double[K];
            prev = values[0];
            for(int i = 1; i <= K; i++)
            {
                double current = prev + step;
                p[i - 1] = distribution.CalculateDistributionFunction(current) - 
                    distribution.CalculateDistributionFunction(prev);
                prev += step;
            }

            double hi2 = 0.0;
            for(int i = 0; i < K; i++)
            {
                hi2 += Math.Pow(frequences[i] - N * p[i], 2) / (N * p[i]);
            }

            return hi2 < PirsonLimit ? true : false;
        }
    }
}
