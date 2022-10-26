using System;

namespace Reflection.Randomness
{
    public class ExponentialDistribution : IContinuousDistribution
    {
        private readonly double _lambda;

        public ExponentialDistribution(double lambda)
        {
            _lambda = lambda;
        }

        public double Generate(Random rnd)
        {
            var u = rnd.NextDouble();
            return -Math.Log(u) / _lambda;
        }
    }
}
