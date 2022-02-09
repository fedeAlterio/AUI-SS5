using Assets.Scripts.Environment.Path.BuildingStrategies.Configuration;
using Assets.Scripts.Models.Path.Blocks;
using Assets.Scripts.Models.Path.Generation.Line;
using Assets.Scripts.Path.BuildingStrategies.Path;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Environment.Path.BuildingStrategies.Path
{
    public class ThinPathStrategy : DifficultyDependentPathStrategy
    {
        // Editor fields
        [SerializeField][Range(0, 1)] private float _easyPathSize = 0.8f;
        [SerializeField][Range(0, 1)] private float _mediumPathSize = 0.6f;
        [SerializeField][Range(0, 1)] private float _hardPathSize = 0.4f;
        [SerializeField] private float _pathLength = 25;


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
            return line
                .GoWithThinPath(Vector3.forward * _pathLength, _easyPathSize)
                ;
        }

        private ILineBuilder<CurveBlock> BuildMedium(ILineBuilder<CurveBlock> line, IPathConfiguration pathConfiguration)
        {
            return line
                .GoWithThinPath(Vector3.forward * _pathLength, _mediumPathSize)
                ;
        }

        private ILineBuilder<CurveBlock> BuildHard(ILineBuilder<CurveBlock> line, IPathConfiguration pathConfiguration)
        {
            return line
                 .GoWithThinPath(Vector3.forward * _pathLength, _hardPathSize)
                 ;
        }
    }
}
