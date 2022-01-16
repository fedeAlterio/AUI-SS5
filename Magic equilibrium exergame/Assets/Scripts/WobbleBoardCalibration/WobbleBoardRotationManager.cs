using Assets.Scripts.Abstractions;
using Assets.Scripts.DependencyInjection.Extensions;
using Assets.Scripts.PlayerMovement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.WobbleBoardCalibration
{
    public class WobbleBoardRotationManager : MonoBehaviour
    {
        // Private fields
        private WobbleboardInput _wobbleboardInput;



        // Initialization
        private void Awake()
        {
            _wobbleboardInput = FindObjectOfType<WobbleboardInput>();
        }



        // Core
        private void Update()
        {
            transform.localRotation = GetRotation();
        }



        // Utils
        private Quaternion GetRotation()
        {
            var zEuler = Mathf.Rad2Deg * _wobbleboardInput.ZAngle;
            var xEuler = Mathf.Rad2Deg * _wobbleboardInput.XAngle;
            return Quaternion.Euler(new Vector3(zEuler, 0, xEuler));
        }
    }
}
