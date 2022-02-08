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
        private IWobbleboardDataProvider _wobbleboardService;
        private IWobbleBoardConfiguration _wobbleBoardConfiguration;
        private SmoothingConfiguration _smoothingConfiguration;
        private Vector2Smoother _smoother;


        // Initialization
        private void Start()
        {
            _wobbleboardService = this.GetInstance<IWobbleboardDataProvider>();
            _wobbleBoardConfiguration = this.GetInstance<IWobbleBoardConfiguration>();
            _smoothingConfiguration = new SmoothingConfiguration
            {
                Speed = 6,
                SmoothingSensibility = (Mathf.PI / 2) / Mathf.Max(1, _wobbleBoardConfiguration.Sensibility)
            };
            _smoother = new Vector2Smoother(this, _smoothingConfiguration, center: Vector2.zero,
                toSmoothValueGetter: () => new Vector2(_wobbleboardService.XAngle, _wobbleboardService.ZAngle));
        }

        private void Update()
        {
            _smoothingConfiguration.SmoothingSensibility = (Mathf.PI / 2) / Mathf.Max(1, _wobbleBoardConfiguration.Sensibility);
        }



        // Properties
        public float XAngle => Mathf.Clamp(_smoother.Value.x, -Mathf.PI / 2, + Mathf.PI / 2);
        public float ZAngle => Mathf.Clamp(_smoother.Value.y, -Mathf.PI / 2, +Mathf.PI / 2);
    }
}
