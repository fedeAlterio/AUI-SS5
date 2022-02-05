using Assets.Scripts.Abstractions;
using Assets.Scripts.Path.BuildingStrategies;
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
        // Editor fields
        [SerializeField] [Range(-90, 90)] private float _horizontalAngle;



        // Private fields
        private WobbleboardInput _wobbleboardInput;
        private WobbleBoardConfiguration _wobbleBoardConfiguration;



        // Initialization
        private void Awake()
        {
            _wobbleboardInput = FindObjectOfType<WobbleboardInput>();
        }

        private void Start()
        {
            _wobbleBoardConfiguration = this.GetInstance<WobbleBoardConfiguration>();
        }



        // Core
        private void Update()
        {
            transform.localRotation = GetRotation();
            _wobbleBoardConfiguration.HorizontalRotationAngle = _horizontalAngle / 180 * Mathf.PI;
            Debug.Log(new { _wobbleboardInput.XAngle, _wobbleboardInput.ZAngle });
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
