using Assets.Scripts.Abstractions;
using Assets.Scripts.Path.BuildingStrategies;
using Assets.Scripts.PlayerMovement.Smoothing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.PlayerMovement
{
    public class WobbleboardInput : MonoBehaviour
    {
        // Private fields
        private IWobbleboardService _wobbleboardService;
        private Vector2Smoother _smoother;



        // Initialization
        private void Start()
        {
            _wobbleboardService = this.GetInstance<IWobbleboardService>();
            var smoothingConfiguration = new SmoothingConfiguration
            {
                Speed = 3,
                SmoothingSensibility = (Mathf.PI / 2) / 12f
            };
            _smoother = new Vector2Smoother(this, smoothingConfiguration, center: Vector2.zero,
                toSmoothValueGetter: () => new Vector2(_wobbleboardService.XAngle, _wobbleboardService.ZAngle));
        }



        // Properties
        public float XAngle => Mathf.Clamp(_smoother.Value.x, -Mathf.PI / 2, + Mathf.PI / 2);
        public float ZAngle => Mathf.Clamp(_smoother.Value.y, -Mathf.PI / 2, +Mathf.PI / 2);
    }
}
