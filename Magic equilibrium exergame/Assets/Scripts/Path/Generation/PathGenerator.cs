using Assets.Scripts.CheckPointSystem;
using Assets.Scripts.DependencyInjection.Extensions;
using Assets.Scripts.Models.Path;
using Assets.Scripts.Models.Path.Blocks;
using Assets.Scripts.Models.Path.Generation;
using Assets.Scripts.Models.Path.Generation.Line;
using Assets.Scripts.Models.Path.Generation.Surface;
using Assets.Scripts.Path.BuildingStrategies.Configuration;
using Assets.Scripts.Path.BuildingStrategies.Levels;
using Assets.Scripts.Path.BuildingStrategies.Path;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Path.Generation
{
    public class PathGenerator : MonoBehaviour
    {
        // Events
        public event Action<ParametricCurve> PathGenerated;



        // Editor fields
        [SerializeField] [CannotBeNull] private PathManager _pathManager;
        [SerializeField] [CannotBeNull] private LevelBuilder _levelBuilder;



        // Initialization
        private void Awake()
        {
        }

        private void Start()
        {
            GenerateLevel();
        }



        // Events
        public ParametricCurve PathCurve { get; private set; }



        // Public
        public void GenerateLevel()
        {
            _pathManager.Clear();

            var pathConfiguration = this.GetInstance<IPathConfiguration>();
            var blocks = _levelBuilder.BuildLevel(pathConfiguration).ToList();
            PathCurve = new CurvesUnion(blocks.Select(x => x.Curve));
            _pathManager.AddRange(blocks);
            PathGenerated?.Invoke(PathCurve);
        }
    }
}
