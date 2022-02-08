using Assets.Scripts.Environment.Path.BuildingStrategies.Configuration;
using Assets.Scripts.Models.Path.Blocks;
using Assets.Scripts.Models.Path.Generation.Line;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Path.BuildingStrategies.Path
{
    public class PathWithHolesStrategy : DifficultyDependentPathStrategy
    {
        // Editor Fields
        [SerializeField] private float _pathLength = 20;
        [Space]
        [Header("Easy settings")]
        [SerializeField] private int _easyHolesCouplesCount = 2;
        [SerializeField] private float _easyHoleSize = 0.6f;
        [SerializeField] [Range(0,1)] private float _easyHolePercentageOnStep = .75f;
        [Space]
        [Header("Medium settings")]
        [SerializeField] private int _mediumHolesCouplesCount = 2;
        [SerializeField] private float _mediumHoleSize = 0.6f;
        [SerializeField][Range(0, 1)] private float _mediumHolePercentageOnStep = .75f;
        [Space]
        [Header("Hard settings")]
        [SerializeField] private int _hardHolesCouplesCount = 2;
        [SerializeField] private float _hardHoleSize = 0.6f;
        [SerializeField][Range(0, 1)] private float _hardHolePercentageOnStep = .75f;


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
            return BuildPathWithHoles(line, _hardHolePercentageOnStep, _hardHoleSize, _hardHolesCouplesCount);
        }

        private ILineBuilder<CurveBlock> BuildMedium(ILineBuilder<CurveBlock> line, IPathConfiguration pathConfiguration)
        {
            return BuildPathWithHoles(line, _mediumHolePercentageOnStep, _mediumHoleSize, _mediumHolesCouplesCount);
        }

        private ILineBuilder<CurveBlock> BuildEasy(ILineBuilder<CurveBlock> line, IPathConfiguration pathConfiguration)
        {
            return BuildPathWithHoles(line, _easyHolePercentageOnStep, _easyHoleSize, _easyHolesCouplesCount);
        }


        private ILineBuilder<CurveBlock> BuildPathWithHoles(ILineBuilder<CurveBlock> line, float holePercentageOnStep, float holeSize, int holesCouplesCount)
        {
            var stepLength = _pathLength / (holesCouplesCount * 2);
            var forwardNotHole = Vector3.forward * stepLength * (1 - holePercentageOnStep);
            var forwardHole = Vector3.forward * stepLength * holePercentageOnStep;
            for (var i = 0; i < holesCouplesCount; i++)
                line = line
                    .Go(forwardNotHole)
                    .GoWithHole(forwardHole, 0, holeSize)
                    .Go(forwardNotHole)
                    .GoWithHole(forwardHole, 1 - holeSize * 0.99f, holeSize)
                    ;
            line = line.Go(forwardNotHole);
            return line;
        }
    }
}
