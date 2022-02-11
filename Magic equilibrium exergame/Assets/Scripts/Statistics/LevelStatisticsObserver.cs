using Assets.Scripts.UI.Game_UI;
using Assets.Scripts.UI.Menu;
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
        private InGameMenuManager _inGameMenu;



        // Initialization
        private void Start()
        {
            _inGameTimer = FindObjectOfType<InGameTimer>();
            _inGameMenu = FindObjectOfType<InGameMenuManager>();
            DeathManager.instance.playerDeathEvent.AddListener(OnPlayerDeath);
            _inGameMenu.ExercizeSkipped += () =>
            {
                ExercizesSkipped++;
            };
        }        



        // Poperties
        [field:SerializeField] public int DeathsCount { get; set; }
        [field:SerializeField] public int SecondsCount { get; set; }
        [field: SerializeField] public int ExercizesSkipped { get; set; }



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
