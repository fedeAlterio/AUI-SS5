using Assets.Scripts.DependencyInjection.Extensions;
using Assets.Scripts.Models.Path.Blocks;
using Assets.Scripts.Models.Path.Generation.Line;
using Assets.Scripts.Path.BuildingStrategies.Blocks;
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
        // Initialization
        private void Awake()
        {
            BlocksContainer = FindObjectOfType<BlockContainer>();
        }



        // Properties
        protected BlockContainer BlocksContainer { get; private set; }        
        public string Name => GetType().Name;



        // Core
        public ILineBuilder<CurveBlock> Build(ILineBuilder<CurveBlock> line)
        {
            var pathConfiguration = this.GetInstance<IPathConfiguration>();
            return Build(line, pathConfiguration);
        }
        public abstract ILineBuilder<CurveBlock> Build(ILineBuilder<CurveBlock> line, IPathConfiguration pathConfiguration);
    }
}

