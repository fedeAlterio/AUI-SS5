using Assets.Scripts.Abstractions;
using Assets.Scripts.Animations;
using Assets.Scripts.Path.BuildingStrategies;
using Assets.Scripts.WobbleBoardCalibration;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Calibration
{
    public class CalibrationManager : MonoBehaviour
    {
        // Events
        public event Action<CalibrationEventArgs> StateChanged;



        // Editor fields
        [SerializeField] private int _startPhaseTime = 10;
        [SerializeField] private int _calibrationAngleTime = 10;
        [SerializeField] private int _endPhaseTime = 5;



        // Private fields
        private IWobbleboardDataProvider _woobleBoardService;
        private AsyncOperationManager _calibrationOperation;
        private WobbleBoardConfiguration _wobbleBoardConfiguration;



        // Initialization
        private void Awake()
        {
            _calibrationOperation = new AsyncOperationManager(this);
        }

        private void Start()
        {
            _woobleBoardService = this.GetInstance<IWobbleboardDataProvider>();
            _wobbleBoardConfiguration = this.GetInstance<WobbleBoardConfiguration>();
            _calibrationOperation.New(Calibrate);
        }



        // Public
        public void SubscribeTo(CalibrationPhase calibrationPhase, Action<int> eventHandler) => StateChanged += args =>
        {
            if (args.CalibrationPhase != calibrationPhase)
                return;

            eventHandler?.Invoke(args.PhaseLength);
        };


        // Core
        private async UniTask Calibrate(IAsyncOperationManager manager)
        {
            await StartPhase(manager);
            await ForwardAngleClaibration(manager);
            await BackwardAngleClaibration(manager);
            await HorizontalAngleCalibration(manager);
            await EndPhase(manager);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        private async UniTask EndPhase(IAsyncOperationManager manager)
        {
            var args = new CalibrationEventArgs(CalibrationPhase.End, _calibrationAngleTime);
            StateChanged?.Invoke(args);
            await manager.Delay(TimeSpan.FromSeconds(_endPhaseTime));
        }

        private async UniTask ForwardAngleClaibration(IAsyncOperationManager manager)
        {
            var args = new CalibrationEventArgs(CalibrationPhase.ForwardAngle, _calibrationAngleTime);
            StateChanged?.Invoke(args);
            await manager.Delay(TimeSpan.FromSeconds(_calibrationAngleTime));
            _wobbleBoardConfiguration.MaxForwardlAngle = _woobleBoardService.ZAngle;
            Debug.Log(Mathf.Rad2Deg * _wobbleBoardConfiguration.MaxForwardlAngle);
        }

        private async UniTask BackwardAngleClaibration(IAsyncOperationManager manager)
        {
            var args = new CalibrationEventArgs(CalibrationPhase.BackwardAngle, _calibrationAngleTime);
            StateChanged?.Invoke(args);
            await manager.Delay(TimeSpan.FromSeconds(_calibrationAngleTime));
            _wobbleBoardConfiguration.MaxBackwardlAngle = _woobleBoardService.ZAngle;
            Debug.Log(Mathf.Rad2Deg * _wobbleBoardConfiguration.MaxBackwardlAngle);
        }

        private async UniTask HorizontalAngleCalibration(IAsyncOperationManager manager)
        {
            var args = new CalibrationEventArgs(CalibrationPhase.HorizontalAngle, _calibrationAngleTime);
            StateChanged?.Invoke(args);
            await manager.Delay(TimeSpan.FromSeconds(_calibrationAngleTime));
            _wobbleBoardConfiguration.MaxHorizontalAngle = _woobleBoardService.XAngle;
            Debug.Log(Mathf.Rad2Deg * _wobbleBoardConfiguration.HorizontalRotationAngle);
        }

        private async UniTask StartPhase(IAsyncOperationManager manager)
        {
            var args = new CalibrationEventArgs(CalibrationPhase.Start, _startPhaseTime);
            StateChanged?.Invoke(args);
            await manager.Delay(TimeSpan.FromSeconds(_startPhaseTime));
        }
    }
}
