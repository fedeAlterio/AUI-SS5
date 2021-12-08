using Assets.Scripts.Models.Path.Generation.Line;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Environment
{
    public class TransformMovement : MonoBehaviour
    {
        // Private fields
        private float _currentTime;



        // Properties
        [field:SerializeField] public float Speed { get; set; }
        [field: SerializeField] public Vector3 ForwardDirection { get; private set; } = Vector3.forward;
        public ParametricCurve Trajectory { get; private set; }
        public float TrajectoryPercentage => (_currentTime - Trajectory.MinT) / Trajectory.DeltaTime;       



        // Public Methods
        public void StartMovement(ParametricCurve curve)
        {
            Trajectory = curve + transform.localPosition;
            _currentTime = Trajectory.MinT;
        }



        // Events
        private void Update()
        {            
            _currentTime = Mathf.Clamp(_currentTime + Speed * Time.smoothDeltaTime, Trajectory.MinT, Trajectory.MaxT);
            if (_currentTime >= Trajectory.MaxT)
                _currentTime = Trajectory.MinT;

            transform.localPosition = Trajectory.PointAt(_currentTime);
            var (forward, _, up) = Trajectory.GetLocalBasis(_currentTime);
            transform.localRotation = Quaternion.LookRotation(forward, up);
        }
    }
}
