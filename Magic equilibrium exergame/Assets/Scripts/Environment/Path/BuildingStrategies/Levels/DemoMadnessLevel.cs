using Assets.Scripts.Models.Path.Blocks;
using Assets.Scripts.Models.Path.Generation.Line;
using Assets.Scripts.Path.BuildingStrategies.Blocks;
using Assets.Scripts.Path.BuildingStrategies.Extensions;
using Assets.Scripts.Path.BuildingStrategies.Path;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Path.BuildingStrategies.Levels
{
    public class DemoMadnessLevel : LevelBuilder
    {
        // Properties
        private BlockStrategy Checkpoint => BlocksContainer.Get<CheckPointStrategy>();
        private BlockStrategy Coins => BlocksContainer.Get<CoinsBlockStrategy>();
        private MovingBlockStrategy MovingBlock => BlocksContainer.Get<MovingBlockStrategy>();



        // Level
        protected override IEnumerable<ILineBuilder<CurveBlock>> CreateLevel(IPathConfiguration pathConfiguration)
        {
            yield return NewLine()
                .Go(Vector3.forward * 10)
                .Go(Vector3.forward * 10)
                .With(Checkpoint)
                .Go(new Vector3(0, 1, 2).normalized * 2)
                .Go(new Vector3(0, -1, 2).normalized * 2)
                .Go(new Vector3(0, -1, 2).normalized * 2)
                .Go(new Vector3(0, 1, 2).normalized * 2)
                .GoWithThinPath(Vector3.forward * 3, 1 / 3.0f)
                .GoWithThinPath(new Vector3(-1, 0, 2).normalized * 5, 1 / 3.0f)
                .GoWithThinPath(new Vector3(1, 0, 2).normalized * 5, 1 / 3.0f)
                .Go(Vector3.forward * 6)
                .With(Checkpoint)
                .GoWithHole(Vector3.forward * 3, 0, 0.6f)
                .Go(Vector3.forward * 6)
                .GoWithHole(Vector3.forward * 3, 0.4f, 0.6f)
                .Go(Vector3.forward * 6)
                .GoWithHole(Vector3.forward * 3, 0, 0.6f)
                .Go(Vector3.forward * 6)
                .GoWithHole(Vector3.forward * 3, 0.4f, 0.6f)
                .Go(Vector3.forward * 5)
                .Go(Vector3.forward * 13)
                .With(Coins);


            yield return NewLine()
                 .Go(Vector3.forward * 3)
                 .With(Checkpoint)
                 .Go(new Vector3(0, 1, 2).normalized)
                 .Go(new Vector3(0, -1, 2).normalized)
                 .Go(Vector3.forward * 3)
                 .With(block => MovingBlock.Strategy(block, 5 * new Vector3(0, -1, 2)))
                 .GoWithThinPath(Vector3.forward * 5, 0.4f)
                 .With(block => MovingBlock.Strategy(block, 5 * new Vector3(-1, 0, 2)))
                 .Go(Vector3.forward * 3)
                 .Go(new Vector3(0, 1, 2).normalized * 10)
                 .With(Coins)
                 .Go(new Vector3(1, -2, 3).normalized * 10)
                 .With(Checkpoint)
                 ;

            var (end, direction) = (Vector3.zero, Vector3.zero);

            {
                var start = CurrentEndPosition;
                var startDirection = CurrentEndDirection;
                var openleft = new Vector3(-1, 0, 2).normalized * 5;
                var openRight = new Vector3(-openleft.x, openleft.y - 1, openleft.z);
                var closedLeft = new Vector3(-openleft.x * 2.5f, openleft.y, openleft.z) * 2.4f;
                var closedRight = new Vector3(-closedLeft.x, closedLeft.y, closedLeft.z);

                yield return NewLine(start, startDirection)
                    .GoWithHole(openleft, 0.5f, 1)
                    .GoWithHole(closedLeft, 0.5f, 1)
                    .GoWithHole(Vector3.forward * 3, 0.5f, 1)
                    .With(curve => MovingBlock.Strategy(curve, Vector3.forward * 10 - Vector3.up * 2));

                var platformEnd = CurrentEndPosition;
                var platformEndDirection = CurrentEndDirection;


                yield return NewLine()
                    .Go(Vector3.forward * 20);
                (end, direction) = (CurrentEndPosition, CurrentEndDirection);

                yield return NewLine(start, startDirection)
                    .GoWithHole(openRight, 0, 0.5f)
                    .GoWithHole(closedRight, 0, 0.5f)
                    .GoWithHole(new Vector3(1.2f, 0, 1) * 3, 0, 0.5f);

                var deltaPos = platformEnd - CurrentEndPosition;
                var v = 0.92f * deltaPos + 0.7f * 1.2f * Vector3.up + 0.96f * Vector3.forward - 0.5f * Vector3.right;

                yield return NewLine()
                    .GoWithHole(Vector3.forward * 5, 0, 0.5f)
                    .With(curve => MovingBlock.Strategy(curve, v));
            }

            yield return NewLine(end, direction)
                .Go(new Vector3(-1, 0, 1.25f) * 4)
                .With(Checkpoint)
                .GoWithHole(Vector3.forward * 6, 0.1f, 0.8f)
                .Go(Vector3.forward);

            yield return NewLine(CurrentEndPosition - CurrentEndDirection * 5 - 4 * Vector3.up, CurrentEndDirection)
                .Go(Vector3.forward * 10)
                .GoWithHole(new Vector3(0, 1, 3) * 5, 0.33f, 0.33f)
                .GoWithHole(new Vector3(0, -2, 3) * 5, 0.33f, 0.33f)
                .Go(new Vector3(0, 1, 3) * 3)
                .With(curve => MovingBlock.Strategy(curve, Vector3.forward * 83 - 11 * Vector3.up + 3 * Vector3.right, speed: 20));

        }
    }
}
