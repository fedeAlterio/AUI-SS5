using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Cameras
{
    public class CameraPoint
    {
        // Initialization
        public CameraPoint(int cameraId, int cameraX, int cameraY)
        {
            CameraId = cameraId;
            CameraX = cameraX;
            CameraY = cameraY;
        }



        // Properties
        public int CameraId { get; }
        public int CameraX { get; }
        public int CameraY { get; }
    }
}
