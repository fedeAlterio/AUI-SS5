using Assets.Scripts.Path.BuildingStrategies.Blocks;
using Assets.Scripts.Path.BuildingStrategies.Path;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Path.BuildingStrategies
{
    public class StratgiesUpdater : MonoBehaviour
    {
        // Editor fields
        [SerializeField] private bool _updateStrategies;



        // Events
        private void OnValidate()
        {
            UpdateStrategies();
        }



        // Core
        private void UpdateStrategies()
        {
            if (!_updateStrategies)
                return;

            FindObjectOfType<BlockContainer>().SearchStrategies();
            FindObjectOfType<PathStrategyContainer>().SearchStrategies();
            _updateStrategies = false;
        }
    }
}
