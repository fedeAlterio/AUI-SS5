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
        
        
        
        // Private fields
        private Dictionary<(int i, int j), CameraPoint> _floorMap = new Dictionary<(int i, int j), CameraPoint>();



        // Initialization
        private void Start()
        {
            InvokeRepeating(nameof(DoStuff),0, 10);
        }

        private void DoStuff()
        {
            _floorMap.Clear();
            FloorTexture = new Texture2D(FrontCamera.targetTexture.width, FrontCamera.targetTexture.height);
            InitializeCameraPoints();
            CreateTexture();
        }


        private void CreateTexture()
        {            
            var cameraTextures = _cameras.Select(x => ToTexture2D(x.targetTexture)).ToArray();
            for(var i=0; i < FloorTexture.width; i++)
                for(var j=0; j < FloorTexture.height; j++)
                {
                    var cameraPoint = _floorMap[(i, j)];
                    var color = cameraTextures[cameraPoint.CameraId]
                        .GetPixel(cameraPoint.CameraX, cameraPoint.CameraY);
                    FloorTexture.SetPixel(i, j, color);
                    //FloorTexture.SetPixel(i, j, Color.green);
                }
            FloorTexture.Apply();
        }


        // Properties
        private Camera FrontCamera => _cameras[0];
        public Texture2D FloorTexture { get; private set; } 



        // Core
        private void InitializeCameraPoints()
        {
            var frontTexture = FrontCamera.targetTexture;
            for(var i=0; i <frontTexture.width; i++)
                for(var j=0; j <frontTexture.height; j++)
                {
                    var pointFloorCoordinates = new Vector2(i/(float)frontTexture.width, j/(float)frontTexture.height);
                    var cameraPoint = ComputePoint(pointFloorCoordinates);
                    _floorMap.Add((i,j), cameraPoint);
                }
        }

        private CameraPoint ComputePoint(Vector2 pointFloorCoordinates)
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
            Debug.DrawLine(bottomLeft, bottomLeft + ux, Color.red,2, false);
            Debug.DrawLine(bottomLeft, bottomLeft + uy, Color.red, 2, false);
            Debug.DrawLine(bottomLeft, bottomLeft + 1000000 * uz, Color.red, 2, false);
            Debug.Log(Vector3.Dot(uz, ux));
            Debug.Log(Vector3.Dot(uz, uy));

            var floorWorldWidth = ux.magnitude;
            var floorWorldHeight = floorWorldWidth / _floorAspectRatio;
            var floorWorldCoordinates = bottomLeft + pointFloorCoordinates.x * ux + pointFloorCoordinates.y * floorWorldHeight * uz;
            var cameraToPoint = (floorWorldCoordinates - FrontCamera.transform.position) / 10;
            Debug.DrawLine(floorWorldCoordinates, floorWorldCoordinates - cameraToPoint , Color.green, 2, false);

            for (var i=1; i < _cameras.Count; i++)
            {
                var camera = _cameras[i];
                var cameraWidthPixels = camera.targetTexture.width;
                var cameraHeightPixels = camera.targetTexture.height;

                var point = camera.WorldToViewportPoint(floorWorldCoordinates);      
                if(IsInsideFrustum(point))
                {
                    var x = (int)Mathf.Clamp(point.x * cameraWidthPixels, 0, cameraWidthPixels);
                    var y = (int)Mathf.Clamp(point.y * cameraHeightPixels, 0, cameraHeightPixels);
                    var cameraPoint = new CameraPoint(i,x,y);
                    return cameraPoint;
                }                
            }
            return new CameraPoint(0, 0, 0);
        }


        private bool IsInsideFrustum(Vector3 viewPortCoordinates)
        {
            return -0 <= viewPortCoordinates.x && viewPortCoordinates.x <= 1
                && -0 <= viewPortCoordinates.y && viewPortCoordinates.y <= 1;
        }

        void OnDrawGizmos()
        {
            Quaternion rotation = Quaternion.LookRotation(transform.TransformDirection(1,0,0));
            Matrix4x4 trs = Matrix4x4.TRS(transform.TransformPoint(10,10,10), rotation, Vector3.one);
            Gizmos.matrix = trs;
            Color32 color = Color.blue;
            color.a = 125;
            Gizmos.color = color;
            Gizmos.DrawCube(Vector3.zero, new Vector3(1.0f, 1.0f, 0.0001f));
            Gizmos.matrix = Matrix4x4.identity;
            Gizmos.color = Color.white;
        }



        // Utils
        Texture2D ToTexture2D(RenderTexture rTex)
        {
            Texture2D tex = new Texture2D(512, 512, TextureFormat.RGB24, false);
            // ReadPixels looks at the active RenderTexture.
            RenderTexture.active = rTex;
            tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
            tex.Apply();
            return tex;
        }
    }
}
