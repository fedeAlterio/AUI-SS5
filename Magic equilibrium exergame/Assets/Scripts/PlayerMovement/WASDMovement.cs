using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.PlayerMovement
{
    public class WASDMovement : MonoBehaviour
    {
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
        private void Update()
        {
            //var horizontalAxis = Input.GetAxis("Horizontal");
            //var verticalAxis = Input.GetAxis("Vertical");
            _rigidbody.velocity = 5 * new Vector3(0, _rigidbody.velocity.y, 1);
        }
    }
}
