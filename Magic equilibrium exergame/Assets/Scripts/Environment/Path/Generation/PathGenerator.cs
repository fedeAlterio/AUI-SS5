using Assets.Scripts.CheckPointSystem;
using Assets.Scripts.Models.Path;
using Assets.Scripts.Models.Path.Blocks;
using Assets.Scripts.Models.Path.Generation;
using Assets.Scripts.Models.Path.Generation.Line;
using Assets.Scripts.Models.Path.Generation.Surface;
using Assets.Scripts.Path.BuildingStrategies;
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



        // Private fields
        private IPathConfiguration _pathConfiguration;



        // Initialization
        private void Start()
        {
            _pathConfiguration = this.GetInstance<IPathConfiguration>();
            GenerateLevel();
        }



        // Events
        public ParametricCurve PathCurve { get; private set; }



        // Public
        public void GenerateLevel()
        {
            _pathManager.Clear();

            var blocks = _levelBuilder.BuildLevel(_pathConfiguration).ToList();
            PathCurve = new CurvesUnion(blocks.Select(x => x.Curve));
            _pathManager.AddRange(blocks);
            PathGenerated?.Invoke(PathCurve);
        }
    }
}
