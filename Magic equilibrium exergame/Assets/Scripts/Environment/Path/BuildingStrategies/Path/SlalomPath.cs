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
    public class SlalomPath : DifficultyDependentPathStrategy
    {
        // Editor fields
        [SerializeField] private float _angleFactor = 0.3f;



        // Private fields
        private const int PathLength = 10;


        // Initialization
        private void Awake()
        {
            OnDifficulty(PathDifficulty.Easy, BuildEasy);
            OnDifficulty(PathDifficulty.Medium, BuildMedium);
            OnDifficulty(PathDifficulty.Hard, BuildHard);
        }



        // Properties
        private Vector3 Right { get; set; }
        private Vector3 Left
        {
            get
            {
                var right = Right;
                return new Vector3(-right.x, right.y, right.z);
            }
        }



        // Core
        private ILineBuilder<CurveBlock> BuildEasy(ILineBuilder<CurveBlock> line, IPathConfiguration pathConfiguration)
        {
            Right = new Vector3(_angleFactor, 0, 1).normalized;            
            return BuildSlalom(line, 2, true);
        }

        private ILineBuilder<CurveBlock> BuildMedium(ILineBuilder<CurveBlock> line, IPathConfiguration pathConfiguration)
        {
            Right = new Vector3(_angleFactor * 1.3f, 0, 1).normalized;
            return BuildSlalom(line, 2, true);
        }

        private ILineBuilder<CurveBlock> BuildHard(ILineBuilder<CurveBlock> line, IPathConfiguration pathConfiguration)
        {
            Right = new Vector3(_angleFactor * 1.5f, 0, 1).normalized;
            return BuildSlalom(line, 2, true);
        }



        // Private

        private ILineBuilder<CurveBlock> BuildSlalom(ILineBuilder<CurveBlock> line, int curveCount, bool shoudlStartRight)
        {
            var forwardDirection = line.CurrentDirection;
            var startDeltaPos = 0.5f * PathLength * (shoudlStartRight ? Right : Left);
            line  = line.Go(startDeltaPos);

            var first = line.CurrentDirection.normalized * PathLength;
            var forwardComponent = Vector3.Dot(line.CurrentDirection.normalized, forwardDirection.normalized) * forwardDirection.normalized;
            var second = (forwardComponent + (forwardComponent - first.normalized)) * PathLength;            
            for (var i=0; i < curveCount - 1; i++)
                line = line.Go(second, isTangentSpace: false).Go(first, isTangentSpace: false);
            line = line.Go(second/2, isTangentSpace: false);       
            
            line = line.Go(forwardDirection * 10, isTangentSpace: false);
            return line;
        }
    }
}

