using Assets.Scripts.CheckPointSystem;
using Assets.Scripts.Models.Path.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Path.BuildingStrategies.Blocks
{
    public class CheckPointBlockStrategy : BlockStrategy
    {
        // Private fields
        private int _checkpointId;       


        // Strategy
        protected override CurveBlock ApplyStrategy(CurveBlock curveBlock)
        {
            var checkpoint = curveBlock.gameObject.AddComponent<CheckPoint>();
            curveBlock.gameObject.AddComponent<CheckPointColorManager>();

            var surface = curveBlock.CurveSurface;
            var spawnPosition = surface.GetTopPosition(surface.Surface.UMin, surface.Surface.VMiddle, topOffset: 0.4f);
            checkpoint.spawnPosition = spawnPosition;
            checkpoint.Initialize(_checkpointId++);
            return curveBlock;
        }
    }

}

