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



        // Initialization
        private void Awake()
        {
            _velocityInput = GetComponent<VelocityInput>();
            _rigidbody = GetComponent<Rigidbody>();
        }



        // Events
        private void FixedUpdate()
        {
            var horizontalAxis = Input.GetAxis("Horizontal");
            var verticalAxis = Input.GetAxis("Vertical");
            _rigidbody.velocity = new Vector3(_speed * horizontalAxis, _rigidbody.velocity.y, _speed * verticalAxis);
        }
    }
}
