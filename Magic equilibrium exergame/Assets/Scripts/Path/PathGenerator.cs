using Assets.Scripts.CheckPointSystem;
using Assets.Scripts.Models.Path.Blocks;
using Assets.Scripts.Models.Path.Generation;
using Assets.Scripts.Models.Path.Generation.Line;
using Assets.Scripts.Models.Path.Generation.Surface;
using Assets.Scripts.Path.BuildingStrategies.Levels;
using Assets.Scripts.Path.BuildingStrategies.Path;
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
        [SerializeField] [CannotBeNull] private PathManager _pathManager;
        [SerializeField] [CannotBeNull] private LevelBuilder _levelBuilder;



        private void Start()
        {
            GenerateLevel();
        }



        // Events
        public ParametricCurve PathCurve { get; private set; }



        // Public
        public void GenerateLevel()
        {
            _pathManager.Clear();

            var blocks = _levelBuilder.BuildLevel(new PathConfiguration()).ToList();         
            PathCurve = new CurvesUnion(blocks.Select(x => x.Curve));            
            _pathManager.AddRange(blocks);
            PathGenerated?.Invoke(PathCurve);
        }


        private IEnumerable<ILineBuilder<CurveBlock>> CreateLevel()
        {
            yield break;
            //yield return NewLine()
            //    .Go(Vector3.forward * 10)
            //    .Go(Vector3.forward * 10)
            //    .With(NewCheckpoint)
            //    .Go(new Vector3(0, 1, 2).normalized * 2)
            //    .Go(new Vector3(0, -1, 2).normalized * 2)
            //    .Go(new Vector3(0, -1, 2).normalized * 2)
            //    .Go(new Vector3(0, 1, 2).normalized * 2)
            //    .GoWithThinPath(Vector3.forward * 3, 1/3.0f)
            //    .GoWithThinPath(new Vector3(-1, 0, 2).normalized * 5, 1 / 3.0f)
            //    .GoWithThinPath(new Vector3(1, 0, 2).normalized * 5, 1 / 3.0f)
            //    .Go(Vector3.forward * 6)
            //    .With(NewCheckpoint)
            //    .GoWithHole(Vector3.forward * 3, 0, 0.6f)
            //    .Go(Vector3.forward * 6)
            //    .GoWithHole(Vector3.forward * 3, 0.4f, 0.6f)
            //    .Go(Vector3.forward * 6)
            //    .GoWithHole(Vector3.forward * 3, 0, 0.6f)
            //    .Go(Vector3.forward * 6)
            //    .GoWithHole(Vector3.forward * 3, 0.4f, 0.6f)
            //    .Go(Vector3.forward * 5)
            //    .Go(Vector3.forward * 13)
            //    .With(_strategies.CoinsPath);


            //yield return NewLine()
            //     .Go(Vector3.forward * 3)
            //     .With(NewCheckpoint)
            //     .Go(new Vector3(0, 1, 2).normalized)
            //     .Go(new Vector3(0, -1, 2).normalized)
            //     .Go(Vector3.forward * 3)
            //     .With(block => NewMovingPlatform(block, 5 * new Vector3(0, -1, 2)))
            //     .GoWithThinPath(Vector3.forward * 5, 0.4f)
            //     .With(block => NewMovingPlatform(block, 5 * new Vector3(-1, 0, 2)))
            //     .Go(Vector3.forward * 3)
            //     .Go(new Vector3(0, 1, 2).normalized * 10)
            //     .With(_strategies.CoinsPath)
            //     .Go(new Vector3(1, -2, 3).normalized * 10)
            //     .With(NewCheckpoint)
            //     ;

            //var (end, direction) = (Vector3.zero, Vector3.zero);

            //{
            //    var start = _lastPathEndPoint;
            //    var startDirection = _lastPathEndDirection;
            //    var openleft = new Vector3(-1, 0, 2).normalized * 5;
            //    var openRight = new Vector3(-openleft.x, openleft.y - 1, openleft.z);
            //    var closedLeft = new Vector3(-openleft.x * 2.5f, openleft.y, openleft.z) * 2.4f;
            //    var closedRight = new Vector3(-closedLeft.x, closedLeft.y, closedLeft.z);

            //    yield return NewLine(start, startDirection)
            //        .GoWithHole(openleft, 0.5f, 1)
            //        .GoWithHole(closedLeft, 0.5f, 1)
            //        .GoWithHole(Vector3.forward * 3, 0.5f, 1)
            //        .With(curve => NewMovingPlatform(curve, Vector3.forward * 10 - Vector3.up * 2));

            //    var platformEnd = _lastPathEndPoint;
            //    var platformEndDirection = _lastPathEndDirection;


            //    yield return NewLine()
            //        .Go(Vector3.forward * 20);
            //    (end, direction) = (_lastPathEndPoint, _lastPathEndDirection);

            //    yield return NewLine(start, startDirection)
            //        .GoWithHole(openRight, 0, 0.5f)
            //        .GoWithHole(closedRight, 0, 0.5f)
            //        .GoWithHole(new Vector3(1.2f,0,1)* 3, 0, 0.5f);

            //    var deltaPos = platformEnd - _lastPathEndPoint;
            //    var v = 0.92f * deltaPos + 0.7f * 1.2f * Vector3.up + 0.96f * Vector3.forward - 0.5f * Vector3.right;
                
            //    yield return NewLine()
            //        .GoWithHole(Vector3.forward * 5, 0, 0.5f)
            //        .With(curve => NewMovingPlatform(curve,  v));                    
            //}

            //yield return NewLine(end, direction)
            //    .Go(new Vector3(-1, 0, 1.25f) * 4)
            //    .With(NewCheckpoint)
            //    .GoWithHole(Vector3.forward * 6, 0.1f, 0.8f)
            //    .Go(Vector3.forward);

            //yield return NewLine(_lastPathEndPoint - _lastPathEndDirection * 5 - 4 * Vector3.up, _lastPathEndDirection)
            //    .Go(Vector3.forward * 10)
            //    .GoWithHole(new Vector3(0,1,3)*5, 0.33f, 0.33f)
            //    .GoWithHole(new Vector3(0, -2, 3) * 5, 0.33f, 0.33f)
            //    .Go(new Vector3(0, 1, 3) * 3)
            //    .With(curve => NewMovingPlatform(curve, Vector3.forward * 83 - 11* Vector3.up + 3*Vector3.right, speed: 20));
        }




        // Block strategy

        //private CurveBlock NewMovingPlatform(CurveBlock curveBlock, Vector3 deltaPosition, float speed = 8f)
        //{
        //    var movingBlock = curveBlock.gameObject.AddComponent<MovingBlock>();
        //    var (z, x, y) = curveBlock.Curve.GetLocalBasis(curveBlock.Curve.MaxT);
        //    movingBlock.DeltaPosition = z * deltaPosition.z + x * deltaPosition.x + y * deltaPosition.y;
        //    movingBlock.Speed = speed;

        //    var movingCurveBlock = Instantiate(_movingCurveBlock);
        //    movingCurveBlock.Initialize(curveBlock);
        //    return movingCurveBlock;
        //}
    }
}
