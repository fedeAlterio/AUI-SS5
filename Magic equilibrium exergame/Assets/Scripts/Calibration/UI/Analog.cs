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
        private WobbleBoardAxis _wobbleBoardAxis;



        // Initialization
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _wobbleBoardAxis = FindObjectOfType<WobbleBoardAxis>();
        }

        private void Start()
        {
            
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
            Position = 0.5f * new Vector2(_wobbleBoardAxis.HorizontalAxis, _wobbleBoardAxis.VerticalAxis);
        }
    }
}
