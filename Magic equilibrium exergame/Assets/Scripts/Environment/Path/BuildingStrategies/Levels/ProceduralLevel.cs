using Assets.Scripts.Models.Path.Blocks;
using Assets.Scripts.Models.Path.Generation.Line;
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
    public class ProceduralLevel : LevelBuilder
    {
        // Path Strategies
        private CheckpointPath Checkpoint => PathStrategyContainer.Get<CheckpointPath>();



        // Generation   
        protected override IEnumerable<ILineBuilder<CurveBlock>> CreateLevel(IPathConfiguration pathConfiguration)
        {
            var allowedStrategies = pathConfiguration.PathStrategiesAllowed.Select(PathStrategyContainer.GetByName);
            var strategies = InjectCheckpoints(allowedStrategies);
            var line = NewLine(start : Vector3.up * 5, direction : Vector3.forward);
            foreach (var strategy in strategies)
                line.GoWith(strategy);
            yield return line;
        }



        // Utils
        private IEnumerable<PathStrategy> InjectCheckpoints(IEnumerable<PathStrategy> pathStrategies)
        {
            foreach(var strategy in pathStrategies)
            {
                yield return strategy;
                yield return Checkpoint;
            }
        }
    }
}
