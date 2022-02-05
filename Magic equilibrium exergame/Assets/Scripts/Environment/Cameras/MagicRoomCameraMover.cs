using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Cameras
{
    public class MagicRoomCameraMover : UnityEngine.MonoBehaviour
    {
        private void Update()
        {
            var euler = transform.rotation.eulerAngles;
            var offset = Input.GetKey(KeyCode.Q) ? 1
                : Input.GetKey(KeyCode.E) ? -1
                : 0;
            transform.rotation = Quaternion.Euler(euler.x + offset, euler.y, euler.z);
            Debug.Log(offset);
        }
    }
}
