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
    public abstract class PathStrategy : MonoBehaviour, IPathStrategy
    {    
        public virtual string Name => GetType().Name;
        public abstract ILineBuilder<CurveBlock> Build(ILineBuilder<CurveBlock> line, PathConfiguration pathConfiguration);
    }
}

