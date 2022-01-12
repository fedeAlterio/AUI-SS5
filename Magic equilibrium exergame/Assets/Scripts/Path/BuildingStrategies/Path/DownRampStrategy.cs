using Assets.Scripts.Models.Path.Blocks;
using Assets.Scripts.Models.Path.Generation.Line;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Path.BuildingStrategies.Path
{
    public class DownRampStrategy : PathStrategy
    {
        public override ILineBuilder<CurveBlock> Build(ILineBuilder<CurveBlock> line, IPathConfiguration pathConfiguration)
        {
            var startDirection = line.CurrentDirection;
            return line
                .Go(new Vector3(0, -1, 3).normalized * 10)
                .Go(startDirection * 5, isTangentSpace: false)
                ;
        }
    }
}
