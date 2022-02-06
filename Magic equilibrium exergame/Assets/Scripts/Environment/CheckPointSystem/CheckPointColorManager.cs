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
            CheckPointManager.instance.CurrentCheckpointChanged += OnCheckpointChanged;
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
        private void OnCheckpointChanged(CheckPoint currentCheckPoint)
        {
            var isEnabled = currentCheckPoint.Id < _checkPoint.Id;
            _colorAnimation.New(manager => ChangeColorTo(isEnabled ? Color.green : _startColor, manager));
        }        



        // Animations
        private async UniTask ChangeColorTo(Color color, IAsyncOperationManager manager)
        {            
            var start = Color;
            await manager.Lerp(start, color, c => Color = c, speed:4);
        }
    }
}
