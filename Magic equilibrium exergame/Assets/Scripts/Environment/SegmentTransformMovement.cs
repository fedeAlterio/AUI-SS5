using Assets.Scripts.Models.Path.Generation.Line;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Environment
{
    public class SegmentTransformMovement : TransformMovement
    {
        // Editor fields    
        [SerializeField] private Vector3 _direction;
        [SerializeField] private float _segmentLength;



        // Initialization
        private void Start()
        {
            StartMovement(Curves.Line(Vector3.zero, _direction * _segmentLength));
        }
    }
}
