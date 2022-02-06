using Assets.Scripts.Models.Path.Blocks;
using Assets.Scripts.Models.Path.Generation.Line;
using Assets.Scripts.Path.BuildingStrategies.Blocks;
using Assets.Scripts.Path.BuildingStrategies.Extensions;
using Assets.Scripts.Path.BuildingStrategies.Path;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Path.BuildingStrategies.Levels
{
    public class DefaultLevelBuilder : LevelBuilder
    {
        protected override IEnumerable<ILineBuilder<CurveBlock>> CreateLevel(IPathConfiguration pathConfiguration)
        {
            var line = NewLine(start: Vector3.up * 5, Vector3.forward);
            var checkpoint = BlocksContainer.Get<CheckPointBlockStrategy>();
            foreach (var strategy in PathStrategyContainer.Strategies.Values)
                line = line.GoWith(strategy, pathConfiguration).With(checkpoint);
            yield return line;
        }
    }
}

