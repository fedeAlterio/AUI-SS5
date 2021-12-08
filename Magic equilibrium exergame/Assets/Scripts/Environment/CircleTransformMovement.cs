﻿using Assets.Scripts.Models.Path.Generation.Line;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Environment
{
    public class CircleTransformMovement : TransformMovement
    {
        // Editor fields    
        [SerializeField] private Vector3 _direction;
        [SerializeField] private float _radiusLength;



        // Initialization
        private void Start()
        {
            var center = _direction * _radiusLength;
            StartMovement(Curves.Circle(center, _radiusLength));
        }
    }
}
