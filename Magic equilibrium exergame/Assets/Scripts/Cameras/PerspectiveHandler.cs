using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.Cameras
{   
    public class PerspectiveHandler : MonoBehaviour
    {
        // Editor fields
        [SerializeField] [CannotBeNull] private List<Camera> _cameras = new List<Camera>();
        [SerializeField] private float _floorAspectRatio = 16 / 9.0f;
        [SerializeField] private Material _customRendererTextureMaterial;


        // Initialization
        private void Start()
        {
            var normalizedFloorToCameraClip = ComputeNormalizedFloorToCameraClip();
            _customRendererTextureMaterial.SetMatrix("_FloorNormalizedToCameraClip", normalizedFloorToCameraClip);
        }

        private Matrix4x4 ComputeNormalizedFloorToCameraClip()
        {

            var perspectiveData = GetPerspectiveData();
            return GetC(_cameras[1]) * GetA(perspectiveData) * GetB(perspectiveData); 
        }

        private PerspectiveData GetPerspectiveData()
        {
            var planeDistance = FrontCamera.nearClipPlane;
            var bottomLeftViewPort = new Vector3(0, 0, planeDistance);
            var bottomRightViewPort = new Vector3(1, 0, planeDistance);
            var topRightViewPort = new Vector3(1, 1, planeDistance);
            var bottomLeft = FrontCamera.ViewportToWorldPoint(bottomLeftViewPort);
            var bottomRight = FrontCamera.ViewportToWorldPoint(bottomRightViewPort);
            var topRight = FrontCamera.ViewportToWorldPoint(topRightViewPort);
            var uy = topRight - bottomRight;
            var ux = bottomRight - bottomLeft;
            var uz = -Vector3.Cross(ux, uy).normalized;


            var floorWorldWidth = ux.magnitude;
            var floorWorldHeight = floorWorldWidth / _floorAspectRatio;
            return new PerspectiveData
            {                
                BottomLeft = bottomLeft,
                Ux = ux,
                Uz = uz,
                FloorWorldHeight = floorWorldHeight
            };
        }


        // Properties
        private Camera FrontCamera => _cameras[0];


        private Matrix4x4 GetA(PerspectiveData perspectiveData)
        {
            return new Matrix4x4(
                new Vector4(1, 0, 0, 0),
                new Vector4(0, 1, 0, 0),
                new Vector4(0, 0, 1, 0),
                new Vector4(perspectiveData.BottomLeft.x, perspectiveData.BottomLeft.y, perspectiveData.BottomLeft.z, 1)
                );
        }

        private Matrix4x4 GetB(PerspectiveData perspectiveData)
        {
            var ux = perspectiveData.Ux;
            var uz = perspectiveData.Uz;
            var c1 = new Vector4(ux.x, ux.y, ux.z, 0);
            var c2 = perspectiveData.FloorWorldHeight* new Vector4(uz.x, uz.y, uz.z, 0);
            var c3 = new Vector4(0, 0, 1, 0);
            var c4 = new Vector4(0, 0, 0, 1);
            return new Matrix4x4(c1, c2, c3, c4);
        }

        private Matrix4x4 GetC(Camera camera)
        {
            Matrix4x4 P = camera.projectionMatrix;
            Matrix4x4 V = camera.transform.worldToLocalMatrix;
            Matrix4x4 scale = Matrix4x4.Scale(new Vector3(-0.5f, -0.5f, 1));
            Matrix4x4 translate = Matrix4x4.Translate(new Vector3(0.5f, 0.5f, 0));
            Matrix4x4 VP = translate * scale * P * V;
            return VP;
        }           

    }
}
