using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.PlayerMovement
{
    public abstract class PlayerMovementAbstract : MonoBehaviour
    {
        // Editor fields
        [SerializeField] private float _speed = 5;



        // Private fields
        private VelocityInput _velocityInput;
        private float _horizontalAxis;
        private float _verticalAxis;



        // Initialization
        protected virtual void Awake()
        {
            _velocityInput = GetComponent<VelocityInput>();
        }


        private void Update()
        {
            _horizontalAxis = HorizontalAxis;
            _verticalAxis = VerticalAxis;
        }



        // properties
        public abstract float HorizontalAxis { get; }
        public abstract float VerticalAxis { get; }


        // Events
        private void FixedUpdate()
        {
            _velocityInput.inputX = _horizontalAxis * 5;
            _velocityInput.inputZ = _verticalAxis * 5;
        }
    }
}
