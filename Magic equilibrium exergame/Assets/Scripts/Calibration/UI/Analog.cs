using Assets.Scripts.Abstractions;
using Assets.Scripts.Path.BuildingStrategies;
using Assets.Scripts.PlayerMovement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Calibration.UI
{
    public class Analog : MonoBehaviour
    {
        // Private fields
        private RectTransform _rectTransform;
        private IMovementAxis _movementAxis;



        // Initialization
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void Start()
        {
            _movementAxis = this.GetInstance<IMovementAxis>();            
        }



        // Properties
        public Vector2 Position
        {
            get => _rectTransform.anchorMin;
            private set
            {                
                var normalizedDirection = new Vector2(0.5f + value.x, 0.5f + value.y);
                _rectTransform.anchorMin =  normalizedDirection;
                _rectTransform.anchorMax = normalizedDirection;
            }
        }



        // Core 
        private void Update()
        {
            Position = 0.5f * new Vector2(_movementAxis.HorizontalAxis, _movementAxis.VerticalAxis);
        }
    }
}
