using System;

namespace Reflection.Randomness
{
    public class NormalDistribution : IContinuousDistribution
    {
        private readonly double _sigma;
        private readonly double _mean;

        public NormalDistribution()
            : this(0.0, 1.0)
        {
        }

        public NormalDistribution(double mean, double sigma)
        {
            _sigma = sigma;
            _mean = mean;
        }

        public double Generate(Random rnd)
        {
            var u = rnd.NextDouble();
            var v = rnd.NextDouble();
            var x = Math.Sqrt(-2 * Math.Log(u)) * Math.Cos(2 * Math.PI * v);
            return x * _sigma + _mean;
        }
    }
}
