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
    public class ForwardPath : PathStrategy
    {
        public override ILineBuilder<CurveBlock> Build(ILineBuilder<CurveBlock> line, IPathConfiguration pathConfiguration)
        {
            return line.Go(Vector3.forward * 10, isTangentSpace: false);
        }
    }
}

