using Assets.Scripts.Abstractions;
using Assets.Scripts.Calibration;
using Assets.Scripts.Path.BuildingStrategies;
using Assets.Scripts.WobbleBoardCalibration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.PlayerMovement
{
    [RequireComponent(typeof(WobbleboardInput))]
    public class WobbleboardMovement : PlayerMovementAbstract
    {
        // Private fields
        private WobbleBoardAxis _wobbleBoardAxis;



        // Initialization
        protected override void Awake()
        {
            base.Awake();
            _wobbleBoardAxis = FindObjectOfType<WobbleBoardAxis>();
        }



        // Properties
        public override float HorizontalAxis => _wobbleBoardAxis.HorizontalAxis;
        public override float VerticalAxis => _wobbleBoardAxis.VerticalAxis;
    }
}
