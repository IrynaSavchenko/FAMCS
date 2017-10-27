using System;

namespace Iism_3
{
    public class NormalDistributionRule : BaseDistribuionRule
    {
        private const int Twelve = 12; //Acceptable Gauss Precision
        private const int Six = 6;

        private double m;
        private double s2;

        public NormalDistributionRule(double m, double s2, int N) : base(N)
        {
            this.m = m;
            this.s2 = s2;
            GenerateValues();
        }

        private void GenerateValues()
        {
            for(int i = 0; i < N; i++)
            {
                for(int j = 0; j < Twelve; j++)
                {
                    Values[i] += RandObj.NextDouble();
                }
                Values[i] -= Six;
                Values[i] = m + Math.Sqrt(s2) * Values[i];
            }
        }

        protected override double? GetTrueMathExpectation()
        {
            return m;
        }

        protected override double? GetTrueDispersion()
        {
            return s2;
        }

        public override double CalculateDistributionFunction(double x)
        {
            return 0.5 * (1 + Meta.Numerics.Functions.AdvancedMath.Erf((x - m) / Math.Sqrt(2 * s2)));
        }
    }
}
