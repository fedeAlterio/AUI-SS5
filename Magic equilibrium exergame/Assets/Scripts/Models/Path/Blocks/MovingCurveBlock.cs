using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Models.Path.Blocks
{
    public class MovingCurveBlock : CurveBlock
    {
        // Private fields
        private CurveBlock _curveBlock;
        private MovingBlock _movingBlock;


        // Initialization
        public void Initialize(CurveBlock curveBlock)
        {
            _curveBlock = curveBlock;
            _movingBlock = curveBlock.GetComponent<MovingBlock>();
            curveBlock.transform.parent = transform;
            CurveSurface = curveBlock.CurveSurface;
        }



        // Properties
        public override Vector3 ExitPosition => base.ExitPosition + _movingBlock.DeltaPosition;
    }
}
