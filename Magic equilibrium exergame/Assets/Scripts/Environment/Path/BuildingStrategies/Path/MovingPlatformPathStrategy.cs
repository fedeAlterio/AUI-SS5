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
    public class MovingPlatformPathStrategy : DifficultyDependentPathStrategy
    {
        // Editor fields
        [SerializeField] private float _easyLength = 10;
        [SerializeField] private float _mediumLength = 7;
        [SerializeField] private float _hardLength = 4;



        // Initialization
        private void Awake()
        {
            OnDifficulty(PathDifficulty.Easy, BuildEasy);
            OnDifficulty(PathDifficulty.Medium, BuildMedium);
            OnDifficulty(PathDifficulty.Hard, BuildHard);
        }



        // Core
        private ILineBuilder<CurveBlock> BuildEasy(ILineBuilder<CurveBlock> line, IPathConfiguration pathConfiguration)
        {
            return BuildMovingPath(line, _easyLength);
        }

        private ILineBuilder<CurveBlock> BuildMedium(ILineBuilder<CurveBlock> line, IPathConfiguration pathConfiguration)
        {
            return BuildMovingPath(line, _mediumLength);
        }

        private ILineBuilder<CurveBlock> BuildHard(ILineBuilder<CurveBlock> line, IPathConfiguration pathConfiguration)
        {
            return BuildMovingPath(line, _hardLength);
        }


        public ILineBuilder<CurveBlock> BuildMovingPath(ILineBuilder<CurveBlock> line, float length)
        {
            var movingBlock = BlocksContainer.Get<MovingBlockStrategy>();
            var randomY = UnityEngine.Random.Range(0, 0.3f);
            var randomX = UnityEngine.Random.Range(-0.5f, 0.5f);

            var deltaPosition = new Vector3(randomX, randomY, 1).normalized * length;
            return line
                .Go(Vector3.forward * length)
                .With(b => movingBlock.Strategy(b, deltaPosition * 5))
                ;
        }
    }
}
