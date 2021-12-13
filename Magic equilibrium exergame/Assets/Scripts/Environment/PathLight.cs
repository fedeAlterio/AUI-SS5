using Assets.Scripts.Animations;
using Assets.Scripts.Sun;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace Assets.Scripts.Environment
{
    public class PathLight : MonoBehaviour
    {
        // Editor fields
        

        
        // private fields
        private DayNightCycleManager _dayNightCycle;
        private HDAdditionalLightData _light;
        private float _intensity;
        private AsyncOperationManager _lightAnimation;
        private bool _isDark;


        // Initialization
        private void Awake()
        {
            _dayNightCycle = FindObjectOfType<DayNightCycleManager>();
            _light = GetComponent<HDAdditionalLightData>();
            _lightAnimation = new AsyncOperationManager(this);
        }


        private void Start()
        {
            _intensity = _light.intensity;
            _light.intensity = 0;
            _isDark = false;
        }



        // Events
        private void Update()
        {
            var isDark = _dayNightCycle.IsDark;
            bool isDarkChanged = _isDark != isDark;
            _isDark = isDark;
            if (isDarkChanged)
                _lightAnimation.New(ChangeLightStatus);
        }


        // Animations
        private async UniTask ChangeLightStatus(IAsyncOperationManager manager)
        {
            var startIntensity = _light.intensity;
            var endIntensity = _isDark ? _intensity : 0;
            await manager.Lerp(startIntensity, endIntensity, val => _light.intensity = val, smooth: false, speed: 8000f);
        }
    }
}
