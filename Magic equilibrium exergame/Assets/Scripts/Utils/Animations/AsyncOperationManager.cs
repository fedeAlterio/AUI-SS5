using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Animations
{
    public class AsyncOperationManager : IAsyncOperationManager
    {
        // Private fields
        private UniTask _previousTask;
        private CancellationTokenSource _cancellationTokenSource;
        private MonoBehaviour _bindedMonoBehaviour;
        private Action _defaultFinallyAction;
        private bool _cancelled;
        private UniTaskCompletionSource _pauseTcs;



        // Initialization
        public AsyncOperationManager(MonoBehaviour bindedMonoBehaviour, Action defaultFinally = null)
        {
            _bindedMonoBehaviour = bindedMonoBehaviour;
            SetDefaultFinally(defaultFinally);
        }



        // Properties
        public bool IsRunning { get; private set; }
        public bool Paused => _pauseTcs != null;
        public CancellationToken CancellationToken => _cancellationTokenSource?.Token ?? default;
        public bool Cancelled => CancellationToken.IsCancellationRequested;
        public float DeltaTime => PlayerLoopTiming == PlayerLoopTiming.Update ? Time.deltaTime : Time.fixedDeltaTime;
        public float Speed { get; set; } = 10;
        public float SpeedScale { get; set; } = 1;
        public PlayerLoopTiming PlayerLoopTiming { get; set; } = PlayerLoopTiming.Update;



        // Public                     
        public void SetDefaultFinally(Action finallyAction)
        {
            _defaultFinallyAction = finallyAction;
        }

        public void New(Func<IAsyncOperationManager, UniTask> task, Action @finally = default)
        {
            NewTask(task, @finally).Forget();
        }


        public void Pause()
        {
            if (_pauseTcs != null)
                return;

            _pauseTcs = new UniTaskCompletionSource();
        }

        public void Resume()
        {
            if (IsRunning || !Paused)
                return;

            var tcs = _pauseTcs;
            _pauseTcs = null;
            tcs.TrySetResult();
        }

        public bool Stop()
        {
            _cancelled = true;
            var wasRunning = IsRunning;
            if (wasRunning)
                StopTask().Forget();
            return wasRunning;
        }



        // Private
        private async UniTask StopTask()
        {
            if (_cancellationTokenSource != null) // Is running
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();
                _cancellationTokenSource = null;
                await _previousTask;
                _previousTask = default;
                _cancelled = false;
            }
        }

        private async UniTaskVoid NewTask(Func<IAsyncOperationManager, UniTask> task, Action onCancel)
        {
            StopTask().Forget();
            while (_cancellationTokenSource != null)
                await UniTask.NextFrame();

            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(_bindedMonoBehaviour.GetCancellationTokenOnDestroy());
            _previousTask = TaskWrapper(task.Invoke(this), onCancel);
        }

        private async UniTask TaskWrapper(UniTask task, Action finallyAction)
        {
            try
            {
                IsRunning = true;
                await task;
            }
            catch (MissingReferenceException e)
            {

            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
            finally
            {
                if (_bindedMonoBehaviour && _cancelled)
                {
                    var @finally = finallyAction ?? _defaultFinallyAction;
                    @finally?.Invoke();
                }
                IsRunning = false;
            }
        }



        // Animation manager
        private const float _precision = 0.002f;
        public async UniTask NextFrame()
        {
            do
            {
                if (Cancelled)
                    throw new OperationCanceledException();

                if (Paused)
                    await _pauseTcs.Task;
                await UniTask.NextFrame(PlayerLoopTiming, CancellationToken);
            } while (Paused || !_bindedMonoBehaviour.gameObject.activeSelf);
        }


        // Delay
        public async UniTask Delay(int delay)
        {
            var time = 0f;
            while (!Cancelled && time * 1000 < delay)
            {
                await NextFrame();
                time += DeltaTime;
            }
            if (Cancelled)
                throw new OperationCanceledException();
        }
        
        public UniTask Delay(TimeSpan timeSpan)
        {
            return Delay((int) timeSpan.TotalMilliseconds);
        }

        // Lerp
        public async UniTask Lerp(float from, float to, Action<float> callback, float speed, bool smooth = true)
        {
            var cancellationToken = _cancellationTokenSource.Token;
            var current = from;
            var direction = Mathf.Sign(to - from);
            var (min, max) = from < to ? (from, to) : (to, from);
            while (Math.Abs(current - to) > _precision && !cancellationToken.IsCancellationRequested)
            {
                var deltaTime = DeltaTime;
                current = smooth
                    ? Mathf.Lerp(current, to, deltaTime * speed * SpeedScale)
                    : Mathf.Clamp(current + direction * speed * deltaTime, min, max);

                callback.Invoke(current);
                await NextFrame();
            }

            if (cancellationToken.IsCancellationRequested)
                throw new OperationCanceledException();
            callback.Invoke(to);
        }

        public UniTask Lerp(float from, float to, Action<float> callback, bool smooth = true)
        {
            return Lerp(from, to, callback, Speed, smooth);
        }


        // Vector2
        public UniTask Lerp(Vector2 from, Vector2 to, Action<Vector2> callback, float speed, bool smooth = true)
        {
            return Lerp(0, 1, t => callback(Vector2.Lerp(from, to, t)), speed, smooth);
        }
        public UniTask Lerp(Vector2 from, Vector2 to, Action<Vector2> callback, bool smooth = true)
        {
            return Lerp(from, to, callback, Speed, smooth);
        }


        // Vector3
        public UniTask Lerp(Vector3 from, Vector3 to, Action<Vector3> callback, float speed, bool smooth = true)
        {
            return Lerp(0, 1, t => callback(Vector3.Lerp(from, to, t)), speed, smooth);
        }
        public UniTask Lerp(Vector3 from, Vector3 to, Action<Vector3> callback, bool smooth = true)
        {
            return Lerp(from, to, callback, Speed, smooth);
        }


        // Color
        public UniTask Lerp(Color from, Color to, Action<Color> callback, float speed = 10, bool smooth = true)
        {
            return Lerp(0, 1, t => callback(Color.Lerp(from, to, t)), speed, smooth);
        }

        public UniTask Lerp(Color from, Color to, Action<Color> callback, bool smooth = true)
        {
            return Lerp(from, to, callback, Speed, smooth);
        }

    }
}
