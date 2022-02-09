using Assets.Scripts.Environment.Path.BuildingStrategies.Path;
using Assets.Scripts.Extensions;
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
        private FinishPathStrategy Finish => PathStrategyContainer.Get<FinishPathStrategy>();
        private ForwardPath Forward => PathStrategyContainer.Get<ForwardPath>();


        // Generation   
        protected override IEnumerable<ILineBuilder<CurveBlock>> CreateLevel(IPathConfiguration pathConfiguration)
        {
            var allowedStrategies = pathConfiguration.PathStrategiesAllowed
                .Select(PathStrategyContainer.GetByName)
                .Cyclic()
                .Take(pathConfiguration.Length);
            var strategies = InjectCheckpoints(allowedStrategies);
            var line = NewLine(start: Vector3.up * 7, direction: Vector3.forward)
                .GoWith(Checkpoint);
            foreach (var strategy in strategies)                
                line = line.GoWith(strategy);

            line = line.GoWith(Finish);
            yield return line;
        }



        // Utils
        private IEnumerable<PathStrategy> InjectCheckpoints(IEnumerable<PathStrategy> pathStrategies)
        {
            foreach(var strategy in pathStrategies)
            {
                yield return strategy;
                yield return Forward;
                yield return Checkpoint;
            }
        }
    }
}
