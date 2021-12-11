using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.GameDebug
{
    public class FpsClamp : MonoBehaviour
    {
        // Editor fields
        //[SerializeField] private int _fpsLimit = 144;
        [SerializeField] private int _vSyncCount = 2;


        // Events
        private void Update()
        {
            //Application.targetFrameRate = _fpsLimit;
            QualitySettings.vSyncCount = _vSyncCount;
        }

    }
}
