using System;

namespace Iism_3
{
    public class CauchyDistributionRule : BaseDistribuionRule
    {
        private double m; //parameter of place
        private double c; //parameter of scale

        public CauchyDistributionRule(double m, double c, int N) : base(N)
        {
            this.m = m;
            this.c = c;
            GenerateValues();
        }

        private void GenerateValues()
        {
            for (int i = 0; i < N; i++)
            {
                //double x = GetRandomInRange(-1, 1);
                double y = GetRandomInRange(0, 1);

                Values[i] = m + c * Math.Tan(Math.PI * (y - 0.5));
            }
        }

        protected override double? GetTrueDispersion()
        {
            return null;
        }

        protected override double? GetTrueMathExpectation()
        {
            return null;
        }

        protected override double? GetDispersion()
        {
            return null;
        }

        protected override double? GetMathExpectation()
        {
            return null;
        }

        public override double CalculateDistributionFunction(double x)
        {
            return 0.5 + Math.Atan((x - m) / c) / Math.PI;
        }

        private double GetRandomInRange(double minimum, double maximum)
        {
            return RandObj.NextDouble() * (maximum - minimum) + minimum;
        }
    }
}
