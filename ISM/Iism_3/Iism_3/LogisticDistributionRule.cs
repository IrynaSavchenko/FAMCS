using System;

namespace Iism_3
{
    public class LogisticDistributionRule : BaseDistribuionRule
    {
        private double a;
        private double b;

        public LogisticDistributionRule(double a, double b, int N) : base(N)
        {
            this.a = a;
            this.b = b;
            GenerateValues();
        }

        private void GenerateValues()
        {
            for (int i = 0; i < N; i++)
            {
                double y = RandObj.NextDouble();

                Values[i] = a + b * Math.Log(y / (1 - y));
            }
        }

        protected override double? GetTrueDispersion()
        {
            return Math.Pow(b, 2) * Math.Pow(Math.PI, 2) / 3.0;
        }

        protected override double? GetTrueMathExpectation()
        {
            return a;
        }

        public override double CalculateDistributionFunction(double x)
        {
            return Math.Pow(1 + Math.Exp(-1.0 * (x - a) / b), -1.0);
        }
    }
}
