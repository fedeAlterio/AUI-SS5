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
        public bool GoingForward { get; private set; } = true;



        // Public Methods
        public void StartMovement(ParametricCurve curve)
        {
            Trajectory = curve + transform.localPosition;
            _currentTime = Trajectory.MinT;
        }



        // Events
        private void Update()
        {
            if(GoingForward && Mathf.Approximately(Trajectory.MaxT, _currentTime))
                GoingForward = false;
            else if(!GoingForward && Mathf.Approximately(Trajectory.MinT, _currentTime))
                GoingForward = true;

            var forwardFactor = GoingForward ? 1 : -1;
            _currentTime = Mathf.Clamp(_currentTime + forwardFactor * Speed * Time.smoothDeltaTime, Trajectory.MinT, Trajectory.MaxT);
            Debug.Log(_currentTime);
            transform.localPosition = Trajectory.PointAt(_currentTime);
            var (forward, _, up) = Trajectory.GetLocalBasis(_currentTime);
            forward *= forwardFactor;
            transform.rotation = Quaternion.LookRotation(forward, up) * Quaternion.FromToRotation(Vector3.forward, ForwardDirection);
        }
    }
}
