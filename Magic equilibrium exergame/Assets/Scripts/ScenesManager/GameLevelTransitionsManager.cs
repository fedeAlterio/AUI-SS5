using Assets.Scripts.Animations;
using Assets.Scripts.FinishLine;
using Assets.Scripts.Path.BuildingStrategies;
using Assets.Scripts.Statistics;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.ScenesManager
{
    public class GameLevelTransitionsManager : MonoBehaviour
    {
        // Private fields
        private AsyncOperationManager _endLevelAnimation;
        private LevelStatisticsObserver _levelStatisticsObserver;



        // Initialization
        private void Awake()
        {
            _levelStatisticsObserver = FindObjectOfType<LevelStatisticsObserver>();
            FinishLineManager.FinishLineReached += OnGameEnded;
            _endLevelAnimation = new AsyncOperationManager(this);
        }



        // Events handlers
        private void OnGameEnded()
        {
            _endLevelAnimation.New(TransitionToStatistics);
        }



        // BlockTime
        private async UniTask TransitionToStatistics(IAsyncOperationManager manager)
        {                 
            await GameSceneManager.LoadScene(GameScene.EndMenu, dontDestroyOnChange: _levelStatisticsObserver.gameObject);
        }
    }
}
