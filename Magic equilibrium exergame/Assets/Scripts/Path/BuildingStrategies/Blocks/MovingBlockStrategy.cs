using Assets.Scripts.Models.Path.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Path.BuildingStrategies.Blocks
{
    public class MovingBlockStrategy : BlockStrategy
    {
        // Editor
        [SerializeField] [CannotBeNull] private MovingCurveBlock _movingBlockPrefab;



        // Strategy
        protected override CurveBlock ApplyStrategy(CurveBlock block)
        {
            return Strategy(block, Vector3.forward * 5);
        }

        public CurveBlock Strategy(CurveBlock block, Vector3 deltaPosition, float speed = 8)
        {
            AddName(block);
            block.transform.localPosition = Vector3.zero;
            var movingBlock = block.gameObject.AddComponent<MovingBlock>();
            var (z, x, y) = block.Curve.GetLocalBasis(block.Curve.MaxT);
            movingBlock.DeltaPosition = z * deltaPosition.z + x * deltaPosition.x + y * deltaPosition.y;
            movingBlock.Speed = speed;

            var movingCurveBlock = Instantiate(_movingBlockPrefab);                        
            movingCurveBlock.Initialize(block);
            return movingCurveBlock;
        }
    }
}
