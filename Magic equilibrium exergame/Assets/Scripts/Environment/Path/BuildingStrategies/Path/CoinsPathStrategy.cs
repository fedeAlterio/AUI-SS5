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
    public class CoinsPathStrategy : PathStrategy
    {
        public override ILineBuilder<CurveBlock> Build(ILineBuilder<CurveBlock> line, IPathConfiguration pathConfiguration)
        {
            var coinStrategy = BlocksContainer.Get<CoinsBlockStrategy>();
            return line
                .Go(Vector3.forward * 30)
                .With(coinStrategy)
                ;
        }
    }
}
