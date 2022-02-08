using Assets.Scripts.Abstractions;
using Assets.Scripts.Path.BuildingStrategies;
using Assets.Scripts.WobbleBoardCalibration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Calibration
{
    public class SensibilitySlider : MonoBehaviour
    {
        // Editor fields
        [SerializeField] private Slider _sensibilitySlider;



        // Private fields
        private WobbleBoardConfiguration _wobbleBoardConfiguration;



        // Initialization
        private void Awake()
        {            
        }
        private void Start()
        {
            _wobbleBoardConfiguration = FindObjectOfType<WobbleBoardConfiguration>();
        }



        // Public
        public void OnSensibilityChanged()
        {
            _wobbleBoardConfiguration.Sensibility = Mathf.Max((int)_sensibilitySlider.value, 1);
        }
    }
}
