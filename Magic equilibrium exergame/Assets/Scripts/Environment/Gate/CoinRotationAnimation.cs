using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Gate
{
    public class CoinRotationAnimation : MonoBehaviour
    {
        // Editor fields
        [SerializeField] private Vector3 _rotationDirection;
        [SerializeField] private float _rotationSpeed;



        // Private fields
        private Transform _transform;
        private const int _rotationAngle = 360;



        // initialization
        private void Awake()
        {
            _transform = transform;            
        }


        private void Start()
        {
            _transform.localRotation = Quaternion.Euler(_rotationDirection.normalized * UnityEngine.Random.Range(0, 360));
        }



        // Events
        private void Update()
        {
            var newRotationEuler = _transform.localRotation.eulerAngles + _rotationSpeed * Time.smoothDeltaTime * _rotationDirection.normalized;
            newRotationEuler = new Vector3(newRotationEuler.x % _rotationAngle, newRotationEuler.y % _rotationAngle, newRotationEuler.z % _rotationAngle);
            _transform.localRotation = Quaternion.Euler(newRotationEuler);
        }
    }
}
