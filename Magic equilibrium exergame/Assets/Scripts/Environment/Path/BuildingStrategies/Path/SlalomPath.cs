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
    public class SlalomPath : PathStrategy
    {
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
        private Vector3 StartRight
        {
            get
            {
                var right = Right;
                return new Vector3(right.x / 2, right.y, right.z);
            }
        }

        private Vector3 StartLeft
        {
            get
            {
                var left = Left;
                return new Vector3(left.x / 2, left.y, left.z);
            }
        }



        // Core
        public override ILineBuilder<CurveBlock> Build(ILineBuilder<CurveBlock> line, IPathConfiguration pathConfiguration)
        {
            SetupSlalom(pathConfiguration);
            return BuildSlalom(line, 1, true);
        }

        private void SetupSlalom(IPathConfiguration configuration)
        {
            var right = configuration.Difficulty switch
            {
                _ => new Vector3(2, 0, 2) * 20
            };
            Right = right;
        }

        private ILineBuilder<CurveBlock> BuildSlalom(ILineBuilder<CurveBlock> line, int curveCount, bool shoudlStartRight)
        {
            var forwardDirection = line.CurrentDirection;
            var startDirection = shoudlStartRight ? StartRight : StartLeft;
            line  = line.Go(startDirection);
            var (first, second) = shoudlStartRight ? (Left, Right) : (Right, Left);
            for (var i=0; i < curveCount - 1; i++)
                line = line.Go(first).Go(second);
            line = line.Go(first);       
            
            line = line.Go(forwardDirection * 10, isTangentSpace: false);
            return line;
        }
    }
}

