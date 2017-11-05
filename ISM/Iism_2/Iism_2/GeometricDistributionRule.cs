using System;

namespace Iism_2
{
    public class GeometricDistributionRule : BaseDistribuionRule
    {
        private double p;
        private double q;

        public GeometricDistributionRule(double p, int N) : base(N)
        {
            this.p = p;
            q = 1 - p;
            GenerateValues();
        }

        private void GenerateValues()
        {
            for (int i = 0; i < N; i++)
            {
                Values[i] = Math.Ceiling(Math.Log(RandObj.NextDouble()) / Math.Log(q));
            }
        }

        protected override double? GetTrueDispersion()
        {
            return q / Math.Pow(p, 2);
        }

        protected override double? GetTrueMathExpectation()
        {
            return 1.0 / p;
        }

        public override double CalculateDistributionFunction(double x)
        {
            return p * Math.Pow(q, x);//1.0 - Math.Pow(q, x);
        }
    }
}
