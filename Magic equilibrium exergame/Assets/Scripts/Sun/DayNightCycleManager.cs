using Assets.Scripts.Animations;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace Assets.Scripts.Sun
{
    public class DayNightCycleManager : MonoBehaviour
    {
        // Events
        public event Action DayNightChange;



        // Editor fields
        [SerializeField] private HDAdditionalLightData _sun;
        [SerializeField] private HDAdditionalLightData _moon;
        [SerializeField, Range(0f,1f)] private float _darkFactor;
        [SerializeField] private float _fadeAnimationSpeed = 1f;
        [SerializeField, Range(0,1)] private float _startDayPercentage;
        [SerializeField] private int _updateSunTimePeriod;



        // Private fields
        private AsyncOperationManager _sunMovement;
        private AsyncOperationManager _updateSunMovement;
        private Vector3 _sunStartPosition;
        private float _moonIntensity;



        // Properties
        [field: SerializeField] public float DawnRotation { get; private set; }
        [field: SerializeField] public float SunsetRotation { get; private set; }
        [field: SerializeField] public float SunHorizontalAngle { get; private set; }
        [field: SerializeField] public float DayLength { get; private set; }
        [field:SerializeField] public float NightLength { get; private set; }
        [field: SerializeField] public float CurrentTime { get; private set; }
        public bool IsNightTime { get; private set; }
        public bool IsDark => IsNightTime || CurrentTime > DayLength
            || (DayLength - CurrentTime) < _darkFactor * DayLength
            || CurrentTime < _darkFactor * 0.5f * DayLength;



        // Initialization
        private void Awake()
        {
            _sunMovement = new AsyncOperationManager(this);
            _updateSunMovement = new AsyncOperationManager(this);
        }

        private void Start()
        {
            _moonIntensity = _moon.intensity;
            _sunMovement.New(DayNightCycle);
            _updateSunMovement.New(UpdateSunPosition);
            CurrentTime = _startDayPercentage * DayLength;
        }





        // Core
        private async UniTask DayNightCycle(IAsyncOperationManager manager)
        {
            IsNightTime = false;
            while(true)
            {                
                await TransitionToDay(manager);
                await SunMovement(manager);
                await TransitionToNight(manager);
                await MoonMovement(manager);
            }
        }


        private async UniTask TransitionToNight(IAsyncOperationManager manager)
        {
            IsNightTime = true;
            _sun.gameObject.SetActive(false);
            _moon.gameObject.SetActive(true);
            manager.Lerp(0, _moonIntensity, val => _moon.intensity = val, smooth:false, speed: _fadeAnimationSpeed).Forget();
            CurrentTime = 0;
        }


        private async UniTask TransitionToDay(IAsyncOperationManager manager)
        {
            if(_moon.gameObject.activeSelf)
            {
                _moonIntensity = _moon.intensity;
                await manager.Lerp(_moon.intensity, 0, val => _moon.intensity = val, smooth: false, speed: _fadeAnimationSpeed);
                _moon.gameObject.SetActive(false);
            }
            _sun.gameObject.SetActive(true);
            _sun.transform.localRotation = Quaternion.Euler(SunTrajectory(0));
            IsNightTime = false;
            CurrentTime = 0;
        }



        private async UniTask MoonMovement(IAsyncOperationManager manager)
        {
            CurrentTime = 0;
            var startIntensity = _moonIntensity;
            while (CurrentTime < NightLength)
            {
                await manager.NextFrame();
                _moon.intensity = startIntensity * (1 - CurrentTime / NightLength);
            }
        }


        private async UniTask SunMovement(IAsyncOperationManager manager)
        {
            CurrentTime = 0;
            while(CurrentTime < DayLength)
                await manager.NextFrame();
        }

        private void UpdateSunPosition()
        {
            if (!_sun.gameObject.activeSelf)
                return;

            var sunRotation = SunTrajectory(CurrentTime);
            _sun.transform.localRotation = Quaternion.Euler(sunRotation);
        }


        private async UniTask UpdateSunPosition(IAsyncOperationManager manager)
        {            
            while(true)
            {
                var updatePeriod = Mathf.Max(_updateSunTimePeriod, 60);
                await manager.Delay(updatePeriod);                
                CurrentTime += updatePeriod / 1000.0f;
                if (!IsNightTime)
                    UpdateSunPosition();
            }
        }



        // Curve
        private Vector3 SunTrajectory(float t)
        {
            t /= DayLength;
            t = TimeFix(t);
            var sunHeight = Mathf.Lerp(DawnRotation, SunsetRotation, t);
            return new Vector3(sunHeight, SunHorizontalAngle, 0);
        }


        private float TimeFix(float t)
        {
            return t;
            return Mathf.Sin((t - 0.5f) * Mathf.PI) * 0.5f + 0.5f;
        }
    }
}
