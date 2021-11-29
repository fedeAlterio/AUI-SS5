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
    public interface IAsyncOperationManager
    {
        public UniTask NextFrame();
        public CancellationToken CancellationToken { get; }
        public bool Paused { get; }
        public bool Cancelled { get; }

        UniTask Lerp(float from, float to, Action<float> callback, bool smooth = true);
        UniTask Lerp(float from, float to, Action<float> callback, float speed, bool smooth = true);

        UniTask Lerp(Vector2 from, Vector2 to, Action<Vector2> callback, bool smooth = true);
        UniTask Lerp(Vector2 from, Vector2 to, Action<Vector2> callback, float speed, bool smooth = true);


        UniTask Lerp(Vector3 from, Vector3 to, Action<Vector3> callback, float speed, bool smooth = true);
        UniTask Lerp(Vector3 from, Vector3 to, Action<Vector3> callback, bool smooth = true);

        UniTask Lerp(Color from, Color to, Action<Color> callback, float speed, bool smooth = true);
        UniTask Lerp(Color from, Color to, Action<Color> callback, bool smooth = true);
        public UniTask Delay(int delayMs);
        public float DeltaTime { get; }
        public float Speed { get; set; }
        public float SpeedScale { get; set; }
    }
}
