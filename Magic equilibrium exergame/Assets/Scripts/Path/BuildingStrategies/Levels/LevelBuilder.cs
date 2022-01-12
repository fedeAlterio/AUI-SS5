using Assets.Scripts.DependencyInjection.Extensions;
using Assets.Scripts.Models.Path.Blocks;
using Assets.Scripts.Models.Path.Generation;
using Assets.Scripts.Models.Path.Generation.Line;
using Assets.Scripts.Models.Path.Generation.Surface;
using Assets.Scripts.Path.BuildingStrategies.Blocks;
using Assets.Scripts.Path.BuildingStrategies.Path;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Path.BuildingStrategies.Levels
{
    public abstract class LevelBuilder : MonoBehaviour, ILevelBuilder
    {
        // Editor fields
        [SerializeField] private Material _pathMaterial;



        // Initialization
        private void Awake()
        {
            BlocksContainer = FindObjectOfType<BlockContainer>();
            PathStrategyContainer = FindObjectOfType<PathStrategyContainer>();
        }


        // Properties
        protected Vector3 CurrentEndPosition { get; set; }
        protected Vector3 CurrentEndDirection { get; set; } = Vector3.forward;
        protected IPathConfiguration PathConfiguration { get; private set; }
        protected BlockContainer BlocksContainer { get; private set; }
        protected PathStrategyContainer PathStrategyContainer { get; private set; }



        // Core
        public IEnumerable<CurveBlock> BuildLevel(IPathConfiguration pathConfiguration)
        {
            PathConfiguration = pathConfiguration;
            foreach(var line in CreateLevel(pathConfiguration))
            {
                var blocks = line.Build();
                CurrentEndPosition = blocks[blocks.Count - 1].ExitPosition;
                CurrentEndDirection = blocks[blocks.Count - 1].ExitDirection;
                foreach(var block in blocks)
                    yield return block;
            }
        }

        protected abstract IEnumerable<ILineBuilder<CurveBlock>> CreateLevel(IPathConfiguration pathConfiguration);



        // protected
        protected ILineBuilder<CurveBlock> NewLine()
        {
            return NewLine(CurrentEndPosition, CurrentEndDirection);
        }

        protected ILineBuilder<CurveBlock> NewLine(Vector3 start, Vector3 direction)
        {
            return PathBuilder<CurveBlock>
                .New(mapper: BlockFromSurface)
                .WithDimensions(PathConfiguration.CurveSize, PathConfiguration.PathThickness, PathConfiguration.PathHeight)
                .WithTextureScaleFactor(PathConfiguration.TextureScale)                
                .Start(start, direction);
        }



        // Private
        private CurveBlock BlockFromSurface(CurveSurface curveSurface)
        {
            var curveBlockPrefab = BlocksContainer.CurveBlockPrefab;
            var curveBlock = BlockFromPrefab(curveBlockPrefab);
            curveBlock.Initialize(curveSurface, _pathMaterial);
            return curveBlock;
        }
        private T BlockFromPrefab<T>(T pathBlock) where T : BaseBlock
        {
            if (pathBlock == null)
                return default;
            var ret = Instantiate(pathBlock);
            ret.name = "Curve";
            ret.transform.parent = transform;
            return ret;
        }
    }
}

