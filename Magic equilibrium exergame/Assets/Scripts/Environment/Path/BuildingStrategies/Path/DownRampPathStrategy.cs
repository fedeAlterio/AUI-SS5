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
    public class DownRampPathStrategy : DifficultyDependentPathStrategy
    {
        // Editor fields
        [Header("Easy")]
        [SerializeField] private Vector3 _easyDirection = new Vector3(0, -1, 3);
        [SerializeField] private float _easyLength = 10;
        [Space]
        [Header("Medium")]
        [SerializeField] private Vector3 _mediumDirection = new Vector3(0, -2, 4);
        [SerializeField] private float _mediumLength = 10;
        [Space]
        [Header("Hard")]
        [SerializeField] private Vector3 _hardDirection = new Vector3(0, -3, 5);
        [SerializeField] private float _hardLength = 10;



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
                .Go(_easyDirection.normalized * _easyLength)
                ;
        }

        private ILineBuilder<CurveBlock> BuildMedium(ILineBuilder<CurveBlock> line, IPathConfiguration pathConfiguration)
        {
            return line
                .Go(_mediumDirection.normalized * _mediumLength)
                ;
        }

        private ILineBuilder<CurveBlock> BuildHard(ILineBuilder<CurveBlock> line, IPathConfiguration pathConfiguration)
        {
            return line
               .Go(_hardDirection.normalized * _hardLength)
               ;
        }
    }
}
