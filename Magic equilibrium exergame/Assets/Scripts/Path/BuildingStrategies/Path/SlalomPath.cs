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
    public class SlalomPath : PathStrategy
    {
        public override ILineBuilder<CurveBlock> Build(ILineBuilder<CurveBlock> line, PathConfiguration pathConfiguration)
        {
            return line
                .Go(new Vector3(1, 0, 2) * 10)
                .Go(new Vector3(-2, 0, 2))
                .Go(new Vector3(1, 0, 2) * 10);           
        }
    }
}

