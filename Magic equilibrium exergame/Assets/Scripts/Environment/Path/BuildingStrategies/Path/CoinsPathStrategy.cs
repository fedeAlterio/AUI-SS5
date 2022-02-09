using Assets.Scripts.Environment.Path.BuildingStrategies.Configuration;
using Assets.Scripts.Models.Path.Blocks;
using Assets.Scripts.Models.Path.Generation.Line;
using Assets.Scripts.Path.BuildingStrategies.Blocks;
using Assets.Scripts.Path.BuildingStrategies.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Path.BuildingStrategies.Path
{
    public class CoinsPathStrategy : DifficultyDependentPathStrategy
    {
        // Editor fields
        [SerializeField] private float _easyLength;
        [SerializeField] private float _mediumLength;
        [SerializeField] private float _hardLength;


        // Initialization
        private void Awake()
        {
            OnDifficulty(PathDifficulty.Easy, BuildEasy);
            OnDifficulty(PathDifficulty.Medium, BuildMedium);
            OnDifficulty(PathDifficulty.Hard, BuildHard);
        }



        // Core
        private ILineBuilder<CurveBlock> BuildHard(ILineBuilder<CurveBlock> line, IPathConfiguration pathConfiguration)
        {
            return BuildPathWithCoins(line,_hardLength);
        }

        private ILineBuilder<CurveBlock> BuildMedium(ILineBuilder<CurveBlock> line, IPathConfiguration pathConfiguration)
        {
            return BuildPathWithCoins(line, _mediumLength);
        }

        private ILineBuilder<CurveBlock> BuildEasy(ILineBuilder<CurveBlock> line, IPathConfiguration pathConfiguration)
        {
            return BuildPathWithCoins(line, _easyLength);
        }
        
        private ILineBuilder<CurveBlock> BuildPathWithCoins(ILineBuilder<CurveBlock> line, float pathLength)
        {
            var coinStrategy = BlocksContainer.Get<CoinsBlockStrategy>();
            return line
                .Go(Vector3.forward * pathLength)
                .With(coinStrategy)
                ;
        }
    }
}
