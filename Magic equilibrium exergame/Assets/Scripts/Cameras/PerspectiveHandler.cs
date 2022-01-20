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

        }

        private void Update()
        {
            ComputeMatrix();
            _customRendererTextureMaterial.SetMatrix("_FloorNormalizedToCameraClip", NormalizedFloorToCameraClip);
        }


        void ComputeMatrix()
        {
            var x = GetPerspectiveData();
            NormalizedFloorToCameraClip = GetC(_cameras[1]) * GetA(x) * GetB(x);
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


        private void CreateTexture()
        {            
            var cameraTextures = _cameras.Select(x => ToTexture2D(x.targetTexture)).ToArray();
            for(var i=0; i < FloorTexture.width; i++)
                for(var j=0; j < FloorTexture.height; j++)
                {
                    var cameraPoint = ComputePointNew(new Vector2(i / (float) FloorTexture.width, j / (float) FloorTexture.height));
                    var color = cameraTextures[cameraPoint.CameraId]
                        .GetPixel(cameraPoint.CameraX, cameraPoint.CameraY);
                    FloorTexture.SetPixel(i, j, color);
                }
            FloorTexture.Apply();
        }


        // Properties
        private Camera FrontCamera => _cameras[0];
        public Texture2D FloorTexture { get; private set; }
        public Matrix4x4 NormalizedFloorToCameraClip { get; private set; }


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
            var c1 = new Vector4(ux.x, ux.y, -ux.z, 0);
            var c2 = perspectiveData.FloorWorldHeight* new Vector4(uz.x, uz.y, uz.z, 0);
            var c3 = new Vector4(0, 0, 1, 0);
            var c4 = new Vector4(0, 0, 0, 1);
            return new Matrix4x4(c1, c2, c3, c4);
        }

        private Matrix4x4 TranslationMatrix(Vector3 v)
        {
            return Matrix4x4.Translate(v);
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
        
        private Vector3 Apply(Matrix4x4 m, Vector3 p)
        {
            var x = new Vector4(p.x, p.y, p.z, 1);
            var result = m * x;
            Vector3 ret = result;
            ret /= result.w;
            return ret;
        }

        private CameraPoint ComputePointNew(Vector2 pointFloorCoordinates)
        {
            var p = GetPerspectiveData();
            var camera = _cameras[1];
            var cameraWidthPixels = camera.targetTexture.width;
            var cameraHeightPixels = camera.targetTexture.height;
            var a = new Vector4(pointFloorCoordinates.x, pointFloorCoordinates.y, 0);
            var point = Apply(NormalizedFloorToCameraClip, a);
            if (IsInsideFrustum(point))
            {
                var x = (int)Mathf.Clamp(point.x * cameraWidthPixels, 0, cameraWidthPixels);
                var y = (int)Mathf.Clamp(point.y * cameraHeightPixels, 0, cameraHeightPixels);
                var cameraPoint = new CameraPoint(1, x, y);
                return cameraPoint;
            }
            return new CameraPoint(0, 0, 0);
        }


        private bool IsInsideFrustum(Vector3 viewPortCoordinates)
        {
            return -0 <= viewPortCoordinates.x && viewPortCoordinates.x <= 1
                && -0 <= viewPortCoordinates.y && viewPortCoordinates.y <= 1;
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
