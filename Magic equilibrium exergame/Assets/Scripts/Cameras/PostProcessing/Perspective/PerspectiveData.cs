using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Cameras
{
    public class PerspectiveData
    {
        public Vector3 BottomLeft { get; set; }
        public Vector3 Ux { get; set; }
        public float FloorWorldHeight { get; set; }
        public Vector3 Uz { get; set; }
    }
}
