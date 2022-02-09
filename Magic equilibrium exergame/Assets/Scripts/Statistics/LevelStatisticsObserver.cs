using Assets.Scripts.UI.Game_UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Statistics
{
    public class LevelStatisticsObserver : MonoBehaviour, ILevelStatistics
    {
        // Private fields
        private InGameTimer _inGameTimer;



        // Initialization
        private void Start()
        {
            _inGameTimer = FindObjectOfType<InGameTimer>();
            DeathManager.instance.playerDeathEvent.AddListener(OnPlayerDeath);            
        }        



        // Poperties
        [field:SerializeField] public int DeathsCount { get; set; }
        [field:SerializeField] public int SecondsCount { get; set; }



        // Event handlers
        private void OnPlayerDeath()
        {
            DeathsCount++;
        }

        private void Update()
        {
            UpdateSeconds();
        }


        // Core
        private void UpdateSeconds()
        {
            if (_inGameTimer == null)
                return;
            SecondsCount = (int)_inGameTimer.ElapsedSeconds;
        }
    }
}
