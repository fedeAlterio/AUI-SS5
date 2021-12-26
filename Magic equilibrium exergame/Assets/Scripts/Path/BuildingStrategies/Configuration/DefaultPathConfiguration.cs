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
        public int Difficulty => 1;
        public int Length => 10;
        public IReadOnlyList<string> PathStrategiesAllowed => new List<string>();
        public float PathThickness { get; } = 4;
        public float CurveSize { get; } = 4;
        public float TextureScale { get; } = 0.25f;
        public float PathHeight { get; } = 1;

    }
}
