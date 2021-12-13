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
        private Vector3 _lastPathEndPoint;
        private Vector3 _lastPathEndDirection = Vector3.forward;
        private List<CurveBlock> _currentPathBlocks = new List<CurveBlock>();


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
                GenerateLevel();
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
        public void GenerateLevel()
        {
            _pathManager.Clear();

            foreach (var line in CreateLevel())
                AddLine(line);

            PathCurve = new CurvesUnion(_currentPathBlocks.Select(x => x.Curve));
            PathGenerated?.Invoke(PathCurve);
        }

        private void AddLine(ILineBuilder<CurveBlock> line)
        {
            var blocks = line.Build();
            var lastCurve = blocks.Last().Curve;
            _lastPathEndPoint = lastCurve.LastPoint;
            _lastPathEndDirection = lastCurve.TangentAt(lastCurve.MaxT);
            if (Mathf.Abs(_lastPathEndDirection.magnitude) < float.Epsilon)
                _lastPathEndDirection = Vector3.forward;

            _currentPathBlocks.AddRange(blocks);
            foreach (var block in blocks)
                _pathManager.Add(block, autoRotation: false);
        }


        private ILineBuilder<CurveBlock> NewLine()
        {
            return PathBuilder<CurveBlock>
                .New(mapper: BlockFromSurface)
                .WithDimensions(CurveSize, PathThickness, PathHeight)
                .WithTextureScaleFactor(TextureScale)
                .Start(_lastPathEndPoint, _lastPathEndDirection);
        }



        // Levels
        private IEnumerable<ILineBuilder<CurveBlock>> CreateLevel()
        {
            yield return NewLine()
                .Go(Vector3.forward * 10)
                .With(NewCheckpoint)
                .Go(new Vector3(0, 1, 2).normalized * 3)
                .Go(new Vector3(0, -1, 2).normalized * 3)
                .GoWithHole(Vector3.forward * 3, 1 / 3.0f, 1 / 3.0f)
                .GoWithHole(new Vector3(-1, 0, 2).normalized * 5, 1 / 3.0f, 2 / 3.0f)
                .GoWithHole(new Vector3(1, 0, 2).normalized * 5, 1 / 3.0f, 2 / 3.0f)
                .Go(Vector3.forward * 5)
                .Go(Vector3.forward * 13)
                .With(_strategies.CoinsPath);


            yield return NewLine()
                 .Go(Vector3.forward * 3)
                 .Go(new Vector3(0, 1, 2).normalized)
                 .Go(new Vector3(0, -1, 2).normalized)
                 .With(NewCheckpoint)
                 .Go(Vector3.forward * 3)
                 .With(block => NewMovingPlatform(block, 5 * new Vector3(0, -1, 2)))
                 .GoWithThinPath(Vector3.forward * 5, 0.4f)
                 .With(block => NewMovingPlatform(block, 5 * new Vector3(-1, 0, 2)))
                 .Go(Vector3.forward * 3)
                 .Go(new Vector3(0, 1, 2).normalized * 20)
                 .With(_strategies.CoinsPath)
                 .GoWithThinPath(new Vector3(0, -1, 3).normalized * 5, 0.4f)
                 .GoWithThinPath(new Vector3(-1, 0, 3).normalized * 5, 0.4f)
                 .GoWithThinPath(new Vector3(1, 0, 3).normalized * 10, 0.4f);
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

        private CurveBlock NewMovingPlatform(CurveBlock curveBlock, Vector3 deltaPos, float speed = 8f)
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
