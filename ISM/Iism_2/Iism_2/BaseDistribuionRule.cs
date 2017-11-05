using System;

namespace Iism_2
{
    public abstract class BaseDistribuionRule
    {
        public int N { get; }
        public double[] ValuesArr
        {
            get
            {
                return (double[])Values.Clone();
            }
        }
        protected double[] Values { get; }
        protected Random RandObj { get; }

        private double? mathExpectation;
        public double? MathExpectation
        {
            get
            {
                mathExpectation = mathExpectation ?? GetMathExpectation();
                return mathExpectation;
            }
        }

        private double? dispersion;
        public double? Dispersion
        {
            get
            {
                dispersion = dispersion ?? GetDispersion();
                return dispersion;
            }
        }

        public double? TrueMathExpectation => GetTrueMathExpectation();
        public double? TrueDispersion => GetTrueDispersion();

        protected BaseDistribuionRule(int N)
        {
            this.N = N;
            Values = new double[N];
            RandObj = new Random();
        }

        protected abstract double? GetTrueMathExpectation();
        protected abstract double? GetTrueDispersion();
        public abstract double CalculateDistributionFunction(double x);

        protected virtual double? GetMathExpectation()
        {
            double mathExpectation = 0.0;
            for (int i = 0; i < N; i++)
            {
                mathExpectation += Values[i];
            }
            return mathExpectation / N;
        }

        protected virtual double? GetDispersion()
        {
            double dispersion = 0.0;
            for (int i = 0; i < N; i++)
            {
                dispersion += Math.Pow(Values[i] - MathExpectation.Value, 2);
            }
            return dispersion / (N - 1);
        }

        //public void Sort()
        //{
        //    Array.Sort(Values);
        //}

        //private double[] CalculateDistributionFunctions()
        //{
        //    double[] arr = new double[N];

        //    for (int i = 0; i < N; i++)
        //    {
        //        arr[i] = CalculateDistributionFunction(Values[i]);
        //    }
        //    return arr;
        //}
    }
}
