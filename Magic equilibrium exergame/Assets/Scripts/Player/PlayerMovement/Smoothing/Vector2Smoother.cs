using Assets.Scripts.Abstractions;
using Assets.Scripts.Animations;
using Assets.Scripts.DependencyInjection.Extensions;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.PlayerMovement.Smoothing
{
    public class Vector2Smoother
    {
        // Private fields
        private readonly MonoBehaviour _monoBehaviour;
        private readonly Func<Vector2> _toSmoothGetter;
        private readonly SmoothingConfiguration _configuration;
        private readonly Vector2 _center;



        // Initialization
        public Vector2Smoother(MonoBehaviour monoBehaviour, SmoothingConfiguration configuration, Vector2 center,
            Func<Vector2> toSmoothValueGetter)
        {
            _configuration = configuration;
            _center = center;
            _monoBehaviour = monoBehaviour;
            _toSmoothGetter = toSmoothValueGetter;
            Update().Forget();
        }



        // Properties
        public Vector2 Value { get; private set; }



        // Core
        private async UniTask Update()
        {
            while (_monoBehaviour)
            {
                SmoothStep();
                await UniTask.NextFrame();
            }
        }

        private void SmoothStep()
        {
            var value = _toSmoothGetter.Invoke();
            var sensibility = _configuration.SmoothingSensibility;
            if (Vector2.Distance(value, Value) < sensibility)
                if (Vector2.Distance(value, _center) > sensibility)
                    return;
                else
                    value = _center;
            Value = Vector2.Lerp(Value, value, _configuration.Speed * Time.deltaTime);
        }
    }
}
