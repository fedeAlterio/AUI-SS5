using Assets.Scripts.Abstractions;
using Assets.Scripts.Path.BuildingStrategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.PlayerMovement
{
    public class PlayerMovement : MonoBehaviour
    {
        // Editor fields
        [SerializeField] private float _speed = 5;



        // Private fields
        private VelocityInput _velocityInput;
        private IMovementAxis _movementAxis;



        // Initialization
        protected virtual void Awake()
        {
            _velocityInput = GetComponent<VelocityInput>();
        }

        private void Start()
        {
            _movementAxis = this.GetInstance<IMovementAxis>();            
        }



        // Events
        private void FixedUpdate()
        {
            _velocityInput.inputX = _movementAxis.HorizontalAxis * _speed;
            _velocityInput.inputZ = _movementAxis.VerticalAxis * _speed;
        }
    }
}
