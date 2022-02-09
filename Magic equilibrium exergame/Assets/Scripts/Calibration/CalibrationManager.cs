using Assets.Scripts.Abstractions;
using Assets.Scripts.Animations;
using Assets.Scripts.Path.BuildingStrategies;
using Assets.Scripts.PlayerMovement;
using Assets.Scripts.ScenesManager;
using Assets.Scripts.WobbleBoardCalibration;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        [SerializeField] private float _maximumDeltaForCalibration = 0.3f;
        [SerializeField] private int _secondsOfStabilityForCalibration = 3;
        [SerializeField] private Image _image;



        // Private fields
        private IWobbleboardDataProvider _wobbleBoardService;
        private IMovementAxis _movementAxis;
        private AsyncOperationManager _calibrationOperation;
        private WobbleBoardConfiguration _wobbleBoardConfiguration;
        private WobbleboardInput _wobbleboardInput;
        private Vector2 _calibrationPivot;
        private DateTime _calibrationPivotDate;



        // Initialization
        private void Awake()
        {
            _wobbleboardInput = FindObjectOfType<WobbleboardInput>();
            _calibrationOperation = new AsyncOperationManager(this);
        }

        private void Start()
        {
            _wobbleBoardService = this.GetInstance<IWobbleboardDataProvider>();
            _wobbleBoardConfiguration = this.GetInstance<WobbleBoardConfiguration>();
            _movementAxis = this.GetInstance<IMovementAxis>();
            _calibrationOperation.New(Calibrate);            
        }



        // Properties
        public Vector2 CurrentAxisDirection => new Vector2(_movementAxis.HorizontalAxis, _movementAxis.VerticalAxis);



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
            await manager.NextFrame();
            await StartPhase(manager);
            await RightAngleCalibration(manager);
            await LeftAngleCalibration(manager);
            await ForwardAngleClaibration(manager);
            await BackwardAngleClaibration(manager);
            await EndPhase(manager);

            DontDestroyOnLoad(_wobbleboardInput.gameObject);
            await GameSceneManager.LoadNextScene();
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
            await WaitUntilStability(acceptacnceTest: () => _wobbleBoardService.ZAngle > 0);
            _wobbleBoardConfiguration.MaxForwardlAngle = _wobbleBoardService.ZAngle;
        }

        private async UniTask BackwardAngleClaibration(IAsyncOperationManager manager)
        {
            var args = new CalibrationEventArgs(CalibrationPhase.BackwardAngle, _calibrationAngleTime);
            StateChanged?.Invoke(args);
            await WaitUntilStability(acceptacnceTest: () => _wobbleBoardService.ZAngle < 0);
            _wobbleBoardConfiguration.MaxBackwardlAngle = _wobbleBoardService.ZAngle;
        }

        private async UniTask RightAngleCalibration(IAsyncOperationManager manager)
        {
            var args = new CalibrationEventArgs(CalibrationPhase.RightAngle, _calibrationAngleTime);
            StateChanged?.Invoke(args);
            await WaitUntilStability(acceptacnceTest: () => _wobbleBoardService.XAngle < 0);
            _wobbleBoardConfiguration.MaxRightAngle = _wobbleBoardService.XAngle;
        }

        private async UniTask LeftAngleCalibration(IAsyncOperationManager manager)
        {
            var args = new CalibrationEventArgs(CalibrationPhase.LeftAngle, _calibrationAngleTime);
            StateChanged?.Invoke(args);
            await WaitUntilStability(acceptacnceTest: () => _wobbleBoardService.XAngle > 0);
            _wobbleBoardConfiguration.MaxLeftAngle = _wobbleBoardService.XAngle;
        }

        private async UniTask StartPhase(IAsyncOperationManager manager)
        {
            var args = new CalibrationEventArgs(CalibrationPhase.Start, _startPhaseTime);
            StateChanged?.Invoke(args);
            await manager.Delay(TimeSpan.FromSeconds(_startPhaseTime));
        }

        private async UniTask WaitUntilStability(Func<bool> acceptacnceTest)
        {
            ChangePivot();
            Color WithAlpha(Color color, float alpha) => new Color(color.r, color.g, color.b, alpha);
            while(DateTime.Now - _calibrationPivotDate < TimeSpan.FromSeconds(_secondsOfStabilityForCalibration))
            {
                var completionPercentage = (DateTime.Now - _calibrationPivotDate).TotalMilliseconds / TimeSpan.FromSeconds(_secondsOfStabilityForCalibration).TotalMilliseconds;
                Debug.Log(new { completionPercentage });
                _image.color = Color.Lerp(WithAlpha(_image.color, 0), WithAlpha(_image.color, 1), (float) completionPercentage);
                var distanceFromPivot = Vector2.Distance(CurrentAxisDirection, _calibrationPivot);
                if (distanceFromPivot > _maximumDeltaForCalibration || !acceptacnceTest())
                    ChangePivot();
                await UniTask.NextFrame(cancellationToken: this.GetCancellationTokenOnDestroy());
            }
        }

        private void ChangePivot()
        {
            _calibrationPivot = CurrentAxisDirection;
            _calibrationPivotDate = DateTime.Now;
        }
    }
}
