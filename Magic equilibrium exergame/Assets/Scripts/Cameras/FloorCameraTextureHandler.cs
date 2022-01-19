using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Cameras
{
    public class FloorCameraTextureHandler : MonoBehaviour
    {
        private PerspectiveHandler _perspectiveHandler;
        [SerializeField] private RenderTexture _renderTexture;


        private void Awake()
        {
             _perspectiveHandler =  FindObjectOfType<PerspectiveHandler>();
        }

        private void Update()
        {
            Graphics.Blit(_perspectiveHandler.FloorTexture, _renderTexture);
        }
    }
}
