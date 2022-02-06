using Assets.Scripts.Animations;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.Game_UI
{
    public class InGameTimer : MonoBehaviour
    {
        // Editor fields
        [SerializeField] private TextMeshProUGUI _label;
        
        
        
        // Private fields
        private int _startTime;
        private AsyncOperationManager _updateTimeOperation;



        // Initialization
        private void Awake()
        {
            _updateTimeOperation = new AsyncOperationManager(this);            
        }

        private void Start()
        {
            StartDateTime = DateTime.Now;
            _updateTimeOperation.New(UpdateTime);
        }



        // Properties
        public DateTime StartDateTime { get; private set; }
        public float ElapsedSeconds { get; private set; }



        // Core
        private async UniTask UpdateTime(IAsyncOperationManager manager)
        {
            while(true)
            {
                _label.text = $"{(int)ElapsedSeconds} s";
                ElapsedSeconds++;
                await manager.Delay(1000);
            }
        }
    }
}
