using Assets.Scripts.Animations;
using Assets.Scripts.FinishLine;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ScenesManager
{
    public class GameLevelTransitionsManager : MonoBehaviour
    {
        // Private fields
        private AsyncOperationManager _endLevelAnimation;



        // Initialization
        private void Awake()
        {
            FinishLineManager.FinishLineReached += OnGameEnded;
            _endLevelAnimation = new AsyncOperationManager(this);
        }



        // Events handlers
        private void OnGameEnded()
        {
            _endLevelAnimation.New(BlockTime);
        }



        // BlockTime
        private async UniTask BlockTime(IAsyncOperationManager manager)
        {
            Time.timeScale = 0;
        }
    }
}
