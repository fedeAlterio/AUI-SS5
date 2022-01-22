using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Cameras
{
    public class CamerasPerspectiveHandler : MonoBehaviour
    {
        // Editor fields
        [SerializeField] private Camera _camera0;
        [SerializeField] private Camera _camera1;
        [SerializeField] private float _fov = 60;
        [SerializeField] private Vector3 _cameraToPlayerOffset;



        // Private fields
        private CameraManager _cameraManager;



        private void Awake()
        {
            _cameraManager = FindObjectOfType<CameraManager>();
        }



        // Events
        private void Update()
        {
            _camera0.fieldOfView = _fov;
            _camera1.fieldOfView = _fov;
            _cameraManager.offset = _cameraToPlayerOffset;
            var eulerAnglesCamera0 = _camera0.transform.localRotation.eulerAngles;                        
            _camera1.transform.localRotation = Quaternion.Euler(eulerAnglesCamera0.x + _fov, eulerAnglesCamera0.y, eulerAnglesCamera0.z);
        }
    }
}
