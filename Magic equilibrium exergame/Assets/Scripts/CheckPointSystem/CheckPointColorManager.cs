using Assets.Scripts.Animations;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.CheckPointSystem
{
    public class CheckPointColorManager : MonoBehaviour
    {
        // Private fields        
        private CheckPoint _checkPoint;
        private Material _material;
        private AsyncOperationManager _colorAnimation;
        private Color _startColor;



        // Initialization
        private void Awake()
        {
            _material = GetComponent<MeshRenderer>().material;
            _checkPoint = GetComponent<CheckPoint>();
            _checkPoint.Taken += OnCheckpointTaken;
            _colorAnimation = new AsyncOperationManager(this);            
            _startColor = Color;
            Color = Color.green;
        }



        // Properties
        public Color Color
        {
            get => _material.GetColor("_" + nameof(Color));
            set => _material.SetColor("_" + nameof(Color), value);
        }

                                    

        // Events
        private void OnCheckpointTaken(CheckPoint checkPoint)
        {
            _colorAnimation.New(ChangeColor);
        }        



        // Animations
        private async UniTask ChangeColor(IAsyncOperationManager manager)
        {
            var start = Color;
            await manager.Lerp(start, _startColor, c => Color = c, speed:4);
        }
    }
}
