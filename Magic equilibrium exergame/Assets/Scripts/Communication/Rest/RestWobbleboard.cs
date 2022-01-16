using Assets.Scripts.Abstractions;
using Assets.Scripts.Communication.Abstractions;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Communication
{
    public class RestWobbleboardService : WobbleboardFetcher
    {
        // Private fields
        private readonly RestHandler _restHandler = new RestHandler(new Uri("http://192.168.1.8"), string.Empty);



        // Core
        protected override async UniTask<T> Get<T>(T responseSchema = default)
            => await _restHandler.PostAndGet(new { requestType = "accelerometerAngles" }, responseSchema, "/");
    }
}
