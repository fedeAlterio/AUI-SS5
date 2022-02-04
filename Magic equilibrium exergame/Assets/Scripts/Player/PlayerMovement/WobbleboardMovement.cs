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



        // Initialization
        protected override void Awake()
        {
            base.Awake();
            _wobbleboardInput = FindObjectOfType<WobbleboardInput>();
        }



        // Properties
        public override float HorizontalAxis => -1 * Mathf.Sin(_wobbleboardInput.XAngle);
        public override float VerticalAxis => Mathf.Sin(_wobbleboardInput.ZAngle);
    }
}
