using Assets.Scripts.Models.Path.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Models.Path.BuildingStrategies
{
    public class BuildingStrategyManager : MonoBehaviour
    {
        // Private fields
        private CoinsPathStrategy _coinsPathStrategy;



        // Initialization
        private void Awake()
        {
            _coinsPathStrategy = FindObjectOfType<CoinsPathStrategy>();
        }



        // Strategies
        public void CoinsPath(CurveBlock curve)
        {
            _coinsPathStrategy.CoinsPath(curve);
        }
    }
}
