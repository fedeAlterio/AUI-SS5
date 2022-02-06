using Assets.Scripts.Models.Path.Blocks;
using Assets.Scripts.Models.Path.Generation.Line;
using Assets.Scripts.Path.BuildingStrategies.Blocks;
using Assets.Scripts.Path.BuildingStrategies.Extensions;
using Assets.Scripts.Path.BuildingStrategies.Levels;
using Assets.Scripts.Path.BuildingStrategies.Path;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Environment.Path.BuildingStrategies.Levels
{
    public class SingleBlockLevelBuilder : LevelBuilder
    {
        // Editor fields
        [SerializeField] private PathStrategy _pathStrategy;



        // Core
        protected override IEnumerable<ILineBuilder<CurveBlock>> CreateLevel(IPathConfiguration pathConfiguration)
        {
            var checkpoint = BlocksContainer.Get<CheckPointBlockStrategy>();
            yield return NewLine()
                .Go(Vector3.forward * 10)
                .With(checkpoint)
                .GoWith(_pathStrategy)
                ;
        }
    }
}
