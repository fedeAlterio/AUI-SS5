using Assets.Scripts.Path.BuildingStrategies.Path;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Path.BuildingStrategies.Configuration
{
    public class DefaultPathConfiguration : IPathConfiguration
    {
        // Initialization
        public DefaultPathConfiguration(IEnumerable<string> allowedStrategiesNames)
        {
            PathStrategiesAllowed = allowedStrategiesNames.ToList();
        }


        // Properties
        public int Difficulty => 1;
        public int Length => 10;
        public IReadOnlyList<string> PathStrategiesAllowed { get; }
        public float PathThickness { get; } = 4;
        public float CurveSize { get; } = 4;
        public float TextureScale { get; } = 0.25f;
        public float PathHeight { get; } = 1;

    }
}
