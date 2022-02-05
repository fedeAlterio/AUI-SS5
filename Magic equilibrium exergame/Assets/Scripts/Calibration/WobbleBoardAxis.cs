using Assets.Scripts.Abstractions;
using Assets.Scripts.Path.BuildingStrategies;
using Assets.Scripts.PlayerMovement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Calibration
{
    public class WobbleBoardAxis : MonoBehaviour
    {
        // Private fields
        private WobbleboardInput _wobbleboardInput;
        private IWobbleBoardConfiguration _wobbleBoardConfiguration;



        // Initialization
        protected void Awake()
        {
            _wobbleboardInput = FindObjectOfType<WobbleboardInput>();
        }

        private void Start()
        {
            _wobbleBoardConfiguration = this.GetInstance<IWobbleBoardConfiguration>();
        }


        // Properties
        public float VerticalAxis
        {
            get
            {
                var rawAngle = _wobbleboardInput.ZAngle;
                var factor = rawAngle > 0 ? _wobbleBoardConfiguration.MaxForwardlAngle : _wobbleBoardConfiguration.MaxBackwardlAngle;
                if (factor == 0)
                    factor = Mathf.PI / 2;
                var normalizedAngle = rawAngle * Mathf.PI * 0.5f / Mathf.Abs(factor);
                normalizedAngle = Mathf.Clamp(normalizedAngle, -Mathf.PI / 2, +Mathf.PI / 2);
                return Mathf.Sin(normalizedAngle);
            }
        }

        public float HorizontalAxis
        {
            get
            {
                var rawAngle = _wobbleboardInput.XAngle;
                var factor = _wobbleBoardConfiguration.MaxHorizontalAngle;
                if (factor == 0)
                    factor = Mathf.PI / 2;
                var normalizedAngle = rawAngle * Mathf.PI * 0.5f / Mathf.Abs(factor);
                normalizedAngle = Mathf.Clamp(normalizedAngle, -Mathf.PI / 2, +Mathf.PI / 2);
                return -1 * Mathf.Sin(normalizedAngle);
            }
        }
    }
}
