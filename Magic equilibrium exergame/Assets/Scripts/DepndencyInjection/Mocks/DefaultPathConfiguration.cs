using Assets.Scripts.Environment.Path.BuildingStrategies.Configuration;
using Assets.Scripts.Path.BuildingStrategies.Path;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.DepndencyInjection.Mocks
{
    public class DefaultPathConfiguration : IPathConfiguration
    {
        // Initialization
        public DefaultPathConfiguration(IEnumerable<string> allowedStrategiesNames)
        {
            PathStrategiesAllowed = allowedStrategiesNames.Distinct().ToList();
        }


        // Properties
        public int Difficulty => (int) PathDifficulty.Hard;
        public int Length => 10;
        public IReadOnlyList<string> PathStrategiesAllowed { get; }
        public float PathThickness { get; } = 4;
        public float CurveSize { get; } = 10;
        public float TextureScale { get; } = 0.25f;
        public float PathHeight { get; } = 1;
    }
}
