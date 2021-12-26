using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Path.BuildingStrategies.Path
{
    public interface IPathConfiguration
    {
        int Difficulty { get; }
        int Length { get; }             
        IReadOnlyList<string> PathStrategiesAllowed { get; }
        float PathThickness { get; }
        float CurveSize { get; } 
        float TextureScale { get; }
        float PathHeight { get; }

    }
}
