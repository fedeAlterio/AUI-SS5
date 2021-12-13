using Assets.Scripts.CheckPointSystem;
using Assets.Scripts.Models.Path.Blocks;
using Assets.Scripts.Models.Path.BuildingStrategies;
using Assets.Scripts.Models.Path.Generation;
using Assets.Scripts.Models.Path.Generation.Line;
using Assets.Scripts.Models.Path.Generation.Surface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Models.Path
{
    public class PathGenerator : MonoBehaviour
    {
        // Events
        public event Action<ParametricCurve> PathGenerated;



        // Editor fields
        [SerializeField] private PathManager _pathManager;
        [SerializeField] private List<BaseBlock> _blocksPrefabs = new List<BaseBlock>();
        [SerializeField] private CurveBlock _curveBlock;
        [SerializeField] private Material _pathMaterial;
        [SerializeField] private MovingCurveBlock _movingCurveBlock;



        // Private fields
        private BuildingStrategyManager _strategies;
        private readonly List<CurveBlock> _blocks = new List<CurveBlock>();
        private int _checkpointId;



        // Initialization
        private void Awake()
        {
            _strategies = FindObjectOfType<BuildingStrategyManager>();
        }

        private void Start()
        {
        }



        // Events
        private void Update()
        {
            if (_oldPathThickness != PathThickness || _oldCurveSize != CurveSize || _oldTextureScale != TextureScale || _oldPathHeight != PathHeight)
            {
                _oldPathHeight = PathHeight;
                _oldPathThickness = PathThickness;
                _oldCurveSize = CurveSize;
                _oldTextureScale = TextureScale;
                GenerateLine();
            }
        }


        // Properties
        private float _oldPathThickness;
        [field: SerializeField] public float PathThickness { get; set; } = 4;


        private float _oldCurveSize;
        [field: SerializeField] public float CurveSize { get; set; } = 4;


        private float _oldTextureScale;
        [field: SerializeField] public float TextureScale { get; set; } = 0.25f;



        private float _oldPathHeight;
        [field: SerializeField] public float PathHeight { get; set; } = 0.1f;


        public ParametricCurve PathCurve { get; private set; }


        // Public
        public void GenerateLine()
        {
            _pathManager.Clear();

            var blocks = PathBuilder<CurveBlock>
                .New(mapper: BlockFromSurface)
                .WithDimensions(CurveSize, PathThickness, PathHeight)
                .WithTextureScaleFactor(TextureScale)
                .Start(Vector3.zero, Vector3.forward)
                .Go(Vector3.forward * 10)
                .With(NewCheckpoint)
                .Go(new Vector3(1,0,2).normalized * 10)
                .With(NewCheckpoint)
                .With(_strategies.CoinsPath)
                .Build();
            PathCurve = new CurvesUnion(blocks.Select(x => x.Curve));

            foreach (var block in blocks)
                _pathManager.Add(block, autoRotation: false);
            PathGenerated?.Invoke(PathCurve);
        }



        // Block strategy
        private CurveBlock NewCheckpoint(CurveBlock curveBlock)
        {
            var checkpoint = curveBlock.gameObject.AddComponent<CheckPoint>();
            curveBlock.gameObject.AddComponent<CheckPointColorManager>();

            var surface = curveBlock.CurveSurface;
            var spawnPosition = surface.GetTopPosition(surface.Surface.UMin, surface.Surface.VMiddle, topOffset: 0.4f);
            checkpoint.spawnPosition = spawnPosition;
            checkpoint.Initialize(_checkpointId++);
            return curveBlock;
        }

        private CurveBlock NewMovingPlatform(CurveBlock curveBlock, Vector3 deltaPos, float speed = 5f)
        {
            var movingBlock = curveBlock.gameObject.AddComponent<MovingBlock>();
            movingBlock.DeltaPosition = deltaPos;
            movingBlock.Speed = speed;

            var movingCurveBlock = Instantiate(_movingCurveBlock);
            movingCurveBlock.Initialize(curveBlock);
            return movingCurveBlock;
        }




        // utils
        private CurveBlock BlockFromSurface(CurveSurface curveSurface)
        {
            var curveBlock = BlockFromPrefab(_curveBlock);
            curveBlock.Initialize(curveSurface, _pathMaterial);
            return curveBlock;
        }

        private T BlockFromPrefab<T>(T pathBlock) where T : BaseBlock
        {
            if (pathBlock == null)
                return default;
            var ret = Instantiate(pathBlock);
            ret.transform.parent = transform;
            //ret.name = pathBlock.BlockName;
            return ret;
        }
    }
}
