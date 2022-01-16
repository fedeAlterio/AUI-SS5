using Assets.Scripts.Abstractions;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Communication.Abstractions
{
    public abstract class WobbleboardFetcher : MonoBehaviour, IWobbleboardService
    {
        // Private fields
        private Thread _fetchingThread;



        // Initialization
        private void Start()
        {
            _fetchingThread = new Thread(FetchDataLoop);    
            IsApplicationRunning = true;
            _fetchingThread.Start();
        }



        // Core
        protected virtual void Update()
        {
            IsApplicationRunning = true;
        }

        protected virtual void OnDestroy()
        {
            IsApplicationRunning = false;
        }

        private async void FetchDataLoop()
        {
            while (IsApplicationRunning)
            {
                try
                {
                    var responseSchema = new { XAngle = 0f, ZAngle = 0f };
                    var (isTimeout, response) = await Get(responseSchema: responseSchema).TimeoutWithoutException(TimeSpan.FromSeconds(1));
                    if(!isTimeout)
                        SetOnMainThread(response.XAngle, response.ZAngle).Forget();
                }
                catch (Exception ex) 
                {                     
                }
            }            
        }

        protected abstract UniTask<T> Get<T>(T responseSchema = default);
        private async UniTaskVoid SetOnMainThread(float xAngle, float zAngle)
        {
            await UniTask.SwitchToMainThread();
            (XAngle, ZAngle) = (xAngle, zAngle);
        }


        // Properties
        public float XAngle { get; private set; }
        public float ZAngle { get; private set; }
        private bool IsApplicationRunning { get; set; } 
    }
}
