using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Reflection.Randomness
{
    public class FromDistribution : Attribute
    {
        public FromDistribution(Type type)
        {
            if (type != typeof(NormalDistribution))
                throw new ArgumentException($"Check declaration parameters of the type: {type.FullName}");
        }

        public FromDistribution(Type type, double lambda)
        {
            if (type != typeof(ExponentialDistribution))
                throw new ArgumentException($"Check declaration parameters of the type: {type.FullName}");
        }

        public FromDistribution(Type type, double mean, double sigma)
        {
            if (type != typeof(NormalDistribution))
                throw new ArgumentException($"Check declaration parameters of the type: {type.FullName}");
        }

        public FromDistribution(Type type, double mean, double sigma, double lambda)
        {
            throw new ArgumentException($"Check declaration parameters of the type: {type.FullName}");
        }
    }

    public class Generator<TType> where TType : new()
    {
        private static PropertyInfo[] Properties { get; set; }

        private readonly Dictionary<PropertyInfo, IContinuousDistribution> _propertiesWithDistributions =
            new Dictionary<PropertyInfo, IContinuousDistribution>();

        public Generator()
        {
            Properties = typeof(TType)
                .GetProperties()
                .Where(p => p.GetCustomAttributes(typeof(FromDistribution), false).Length != 0)
                .ToArray();
        }

        public TType Generate(Random rnd)
        {
            var result = new TType();
            foreach (var property in Properties)
            {
                if (_propertiesWithDistributions.ContainsKey(property))
                {
                    property.SetValue(result, _propertiesWithDistributions[property].Generate(rnd));
                    continue;
                }

                var attributeArgs = property.CustomAttributes.First().ConstructorArguments.ToArray();
                var values = attributeArgs.Skip(1).Select(a => a.Value).ToArray();
                var distribution =
                    Activator.CreateInstance((Type) attributeArgs[0].Value, values) as IContinuousDistribution;
                property.SetValue(result, distribution?.Generate(rnd));
                _propertiesWithDistributions.Add(property, distribution);
            }
            return result;
        }
    }
}