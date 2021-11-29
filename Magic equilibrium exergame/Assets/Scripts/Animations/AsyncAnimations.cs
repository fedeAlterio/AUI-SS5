using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Animations
{
    public static class AsyncAnimations
    {
        // Private fields
        private static float _translationOffset = 20f;

        public static async UniTask FadeTo(CanvasGroup canvasGroup, float to, IAsyncOperationManager animationManager)
        {
            await animationManager.Lerp(canvasGroup.alpha, to, alpha => canvasGroup.alpha = alpha);
        }

        public static async UniTask ScaleTo(Transform transform, float scale, IAsyncOperationManager animationManager)
        {
            await animationManager.Lerp(transform.localScale, Vector3.one * scale, scale => transform.localScale = scale);
        }

        public static UniTask FadeInWithScale(Transform transform, CanvasGroup canvasGroup, IAsyncOperationManager manager, float scaleInValue = 1)
        {
            var fade = FadeTo(canvasGroup, 1, manager);
            var scale = ScaleTo(transform, scaleInValue, manager);
            return UniTask.WhenAll(fade, scale);
        }

        public static UniTask FadeOutWithScale(Transform transform, CanvasGroup canvasGroup, IAsyncOperationManager manager, float scaleOutValue = 0.8f)
        {
            var fade = FadeTo(canvasGroup, 0, manager);
            var scale = ScaleTo(transform, scaleOutValue, manager);
            return UniTask.WhenAll(fade, scale);
        }


        public static async UniTask Translate(Transform transform, Vector3 position, IAsyncOperationManager manager)
        {
            await manager.Lerp(transform.position, position, pos => transform.position = pos);
        }


        public static UniTask FadeInWithTranslation(Transform transform, CanvasGroup canvasGroup, Vector2 startPoint, IAsyncOperationManager manager, float translationDistance = 20)
        {
            var destination = startPoint + translationDistance * Vector2.up;
            var translation = Translate(transform, destination, manager);
            var fade = FadeTo(canvasGroup, 1, manager);
            return UniTask.WhenAll(translation, fade);
        }


        public static UniTask FadeOutWithTranslation(Transform transform, CanvasGroup canvasGroup, Vector2 startPoint, IAsyncOperationManager manager, float translationDistance = 20)
        {
            var destination = startPoint + translationDistance * Vector2.down;
            var translation = Translate(transform, destination, manager);
            var fade = FadeTo(canvasGroup, 0, manager);
            return UniTask.WhenAll(translation, fade);
        }
    }
}
