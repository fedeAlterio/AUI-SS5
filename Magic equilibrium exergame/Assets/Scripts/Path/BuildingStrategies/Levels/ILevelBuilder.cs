using Assets.Scripts.Models.Path.Blocks;
using Assets.Scripts.Models.Path.Generation.Line;
using Assets.Scripts.Path.BuildingStrategies.Path;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Path.BuildingStrategies.Levels
{
    public interface ILevelBuilder
    {
        IEnumerable<CurveBlock> BuildLevel(PathConfiguration pathConfiguration);
    }
}

