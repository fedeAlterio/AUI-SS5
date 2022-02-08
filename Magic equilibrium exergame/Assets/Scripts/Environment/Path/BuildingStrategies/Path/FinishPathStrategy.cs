using Assets.Scripts.Environment.Path.BuildingStrategies.Blocks;
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

namespace Assets.Scripts.Environment.Path.BuildingStrategies.Path
{
    public class FinishPathStrategy : PathStrategy
    {
        public override ILineBuilder<CurveBlock> Build(ILineBuilder<CurveBlock> line, IPathConfiguration pathConfiguration)
        {
            var finishLineStrategy = BlocksContainer.Get<FinishLineBlockStrategy>();
            return line
                .Go(Vector3.forward * 5)
                .With(finishLineStrategy)
                ;
        }
    }
}
