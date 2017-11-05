using System;

namespace Iism_2
{
    public class PoissonDistributionRule : BaseDistribuionRule
    {
        private double lambda;

        public PoissonDistributionRule(double lambda, int N) : base(N)
        {
            this.lambda = lambda;
            GenerateValues();
        }

        private void GenerateValues()
        {
            double p = lambda / N;
            double a = 0;
            double b = 1;

            for (int i = 0; i < N; i++)
            {
                for(int j = 0; j < N; j++)
                {
                    Values[i] += RandObj.NextDouble() < p ? b : a;
                }
            }
        }

        protected override double? GetTrueDispersion()
        {
            return lambda;
        }

        protected override double? GetTrueMathExpectation()
        {
            return lambda;
        }

        public override double CalculateDistributionFunction(double x)
        {
            double rez = 0.0;
            int integerX = (int)x;

            for(int i = 0; i <= integerX; i++)
            {
                rez += Math.Exp(-1.0 * lambda) * Math.Pow(lambda, i) / Factorial(i);
            }
            return rez;
        }

        private int Factorial(int n)
        {
            if (n >= 2) return n * Factorial(n - 1);
            return 1;
        }
    }
}
