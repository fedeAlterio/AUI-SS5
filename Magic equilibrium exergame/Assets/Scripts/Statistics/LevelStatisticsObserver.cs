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
        // Initialization
        private void Start()
        {
            DeathManager.instance.playerDeathEvent.AddListener(OnPlayerDeath);            
        }        



        // Poperties
        [field:SerializeField] public int DeathsCount { get; set; }




        // Event handlers
        private void OnPlayerDeath()
        {
            DeathsCount++;
        }
    }
}
