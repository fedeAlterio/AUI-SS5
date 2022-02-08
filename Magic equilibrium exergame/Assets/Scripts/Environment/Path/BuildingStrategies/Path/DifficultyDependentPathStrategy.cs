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
    public class DifficultyDependentPathStrategy : PathStrategy
    {
        protected delegate ILineBuilder<CurveBlock> PathBuilderDelegate(ILineBuilder<CurveBlock> line, IPathConfiguration pathConfiguration);
        
        
        
        // Fields
        protected Dictionary<int, PathBuilderDelegate> _strategiesByDifficulty = new Dictionary<int, PathBuilderDelegate>();



        // Properties
        protected virtual PathBuilderDelegate FallbackStrategy => _strategiesByDifficulty.Values.FirstOrDefault();



        // Builder
        public override ILineBuilder<CurveBlock> Build(ILineBuilder<CurveBlock> line, IPathConfiguration pathConfiguration)
        {
            Debug.Log(pathConfiguration.Difficulty);
            return _strategiesByDifficulty.TryGetValue(pathConfiguration.Difficulty, out var builder)
                    ? builder.Invoke(line, pathConfiguration)
                    : FallbackStrategy?.Invoke(line, pathConfiguration) ?? line;
        }


        // Protected
        protected void OnDifficulty(int difficulty, PathBuilderDelegate pathBuilder)
        {            
            _strategiesByDifficulty[difficulty] = pathBuilder;
        }

        protected void OnDifficulty(PathDifficulty difficulty, PathBuilderDelegate pathBuilder)
        {
            OnDifficulty((int) difficulty, pathBuilder);
        }
    }
}
