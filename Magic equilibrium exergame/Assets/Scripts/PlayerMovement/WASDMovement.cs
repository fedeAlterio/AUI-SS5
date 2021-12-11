using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.PlayerMovement
{
    [RequireComponent(typeof(Rigidbody))]
    public class WASDMovement : MonoBehaviour
    {
        // Editor fields
        [SerializeField] private float _speed = 5;



        // Private fields
        private VelocityInput _velocityInput;
        private Rigidbody _rigidbody;
        private float _horizontalAxis;
        private float _verticalAxis;



        // Initialization
        private void Awake()
        {
            _velocityInput = GetComponent<VelocityInput>();
            _rigidbody = GetComponent<Rigidbody>();
        }


        private void Update()
        {
            _horizontalAxis = Input.GetAxis("Horizontal");
            _verticalAxis = Input.GetAxis("Vertical");            
        }


        // Events
        private void FixedUpdate()
        {
            _rigidbody.velocity = new Vector3(_speed * _horizontalAxis, _rigidbody.velocity.y, _speed * _verticalAxis);
        }
    }
}
