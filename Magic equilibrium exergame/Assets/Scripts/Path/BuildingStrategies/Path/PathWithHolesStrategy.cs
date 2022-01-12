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
    public class PathWithHolesStrategy : PathStrategy
    {
        public override ILineBuilder<CurveBlock> Build(ILineBuilder<CurveBlock> line, IPathConfiguration pathConfiguration)
        {
            return line
                .GoWithHole(Vector3.forward * 10, 0, 0.5f)
                .Go(Vector3.forward * 10)
                .GoWithHole(Vector3.forward * 10, 0.5f, 0.5f)
                ;
        }
    }
}
