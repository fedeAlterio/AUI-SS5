using Assets.Scripts.Models.Path.Blocks;
using Assets.Scripts.Models.Path.Generation.Line;
using Assets.Scripts.Path.BuildingStrategies.Blocks;
using Assets.Scripts.Path.BuildingStrategies.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Path.BuildingStrategies.Path
{
    public class MovingPlatformPathStrategy : PathStrategy
    {
        public override ILineBuilder<CurveBlock> Build(ILineBuilder<CurveBlock> line, IPathConfiguration pathConfiguration)
        {
            var movingBlock = BlocksContainer.Get<MovingBlockStrategy>();
            return line
                .Go(Vector3.forward * 10)
                .With(b => movingBlock.Strategy(b, Vector3.forward * 20))
                ;
        }
    }
}
