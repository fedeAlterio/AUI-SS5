using Assets.Scripts.Abstractions;
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
        private WobbleboardInput _wobbleboardInput;
        private IWobbleBoardConfiguration _wobbleBoardConfiguration;



        // Initialization
        protected override void Awake()
        {
            base.Awake();
            _wobbleboardInput = FindObjectOfType<WobbleboardInput>();
        }

        private void Start()
        {
            _wobbleBoardConfiguration = this.GetInstance<IWobbleBoardConfiguration>();
        }



        // Properties
        public override float VerticalAxis
        {
            get
            {
                var rawAngle = _wobbleboardInput.ZAngle;
                var factor = rawAngle > 0 ? _wobbleBoardConfiguration.MaxForwardlAngle : _wobbleBoardConfiguration.MaxBackwardlAngle;
                var normalizedAngle = rawAngle * Mathf.PI * 0.5f / factor;
                normalizedAngle = Mathf.Clamp(normalizedAngle, -Mathf.PI / 2, + Mathf.PI / 2);
                Debug.Log(rawAngle);
                return Mathf.Sin(rawAngle);
            }
        }

        public override float HorizontalAxis
        {
            get
            {
                var rawAngle = _wobbleboardInput.XAngle;
                var factor = _wobbleBoardConfiguration.MaxHorizontalAngle;
                var normalizedAngle = rawAngle * Mathf.PI * 0.5f / factor;
                normalizedAngle = Mathf.Clamp(normalizedAngle, -Mathf.PI / 2, +Mathf.PI / 2);
                Debug.Log(rawAngle);
                return -1 * Mathf.Sin(rawAngle);
            }
        }
    }
}
