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
        // Private fields
        private IPathConfiguration _pathConfiguration;



        // Initialization
        protected virtual void Start()
        {
            _pathConfiguration = this.GetInstance<IPathConfiguration>();        
            BlocksContainer = FindObjectOfType<BlockContainer>();
        }



        // Properties
        protected BlockContainer BlocksContainer { get; private set; }        
        public string Name => GetType().Name;



        // Core
        public ILineBuilder<CurveBlock> Build(ILineBuilder<CurveBlock> line)
        {
            return Build(line, _pathConfiguration);
        }
        public abstract ILineBuilder<CurveBlock> Build(ILineBuilder<CurveBlock> line, IPathConfiguration pathConfiguration);
    }
}

