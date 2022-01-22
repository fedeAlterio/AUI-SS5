using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace Assets.Scripts.Cameras
{
    [ExecuteInEditMode]
    public class CameraRenderReplacement : MonoBehaviour
    {
        [SerializeField] private RenderTexture _cameraTexture;

        private void Update()
        {
            CameraTexture = _cameraTexture;
        }

        public static RenderTexture CameraTexture { get; set; }        
    }
}
