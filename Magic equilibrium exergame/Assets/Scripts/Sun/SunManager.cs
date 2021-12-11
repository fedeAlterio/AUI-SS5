using Assets.Scripts.Animations;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Sun
{
    public class SunManager : MonoBehaviour
    {
        // Editor fields
        [SerializeField] private Transform _sunTransform;
        [SerializeField] private int _sunPositionUpdatePeriodMs;



        // Private fields
        private AsyncOperationManager _sunMovement;



        // Properties
        [field: SerializeField] public float DawnRotation { get; private set; }
        [field: SerializeField] public float SunsetRotation { get; private set; }
        [field: SerializeField] public float SunHorizontalAngle { get; private set; }
        [field: SerializeField] public float DayLength { get; private set; }
        [field: SerializeField] public float CurrentTime { get; private set; }



        // Initialization
        private void Awake()
        {
            _sunMovement = new AsyncOperationManager(this);
        }

        private void Start()
        {            
            _sunMovement.New(SunMovement);
        }



        // Events
        private void Update()
        {
            UpdateSunPosition();
            
        }





        // Core
        private async UniTask SunMovement(IAsyncOperationManager manager)
        {
            CurrentTime = 0;
            while(true)
            {
                var updateTime = Mathf.Max(_sunPositionUpdatePeriodMs, 60);
                await manager.Delay(updateTime);
                CurrentTime += updateTime/1000.0f;
            }
        }

        private void UpdateSunPosition()
        {
            var sunRotation = SunTrajectory(CurrentTime);
            _sunTransform.localRotation = Quaternion.Euler(sunRotation);
        }




        // Curve
        private Vector3 SunTrajectory(float t)
        {
            t /= DayLength;
            var sunHeight = Mathf.Lerp(DawnRotation, SunsetRotation, t);
            return new Vector3(sunHeight, SunHorizontalAngle, 0);
        }
    }
}
