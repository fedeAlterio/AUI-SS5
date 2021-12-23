using Assets.Scripts.Models.Path.Blocks;
using Assets.Scripts.Path.BuildingStrategies.Blocks;
using Assets.Scripts.Path.BuildingStrategies.Extensions;
using Assets.Scripts.Path.BuildingStrategies.Path;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Path.BuildingStrategies.Levels
{
    public class DefaultLevelBuilder : LevelBuilder
    {
        public override IEnumerable<CurveBlock> BuildLevel(PathConfiguration pathConfiguration)
        {
            var line = NewLine();
            var checkpoint = _blocksContainer.Get<CheckPointStrategy>();
            foreach (var strategy in _pathStrategiesContainer.Strategies.Values)
                line = line.GoWith(strategy, pathConfiguration).With(checkpoint);
            return line.Build();
        }
    }
}

