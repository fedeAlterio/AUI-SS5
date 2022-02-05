using Assets.Scripts.Models.Path.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Path.BuildingStrategies.Blocks
{
    public class GameObjectOverPlatformBlockStrategy : BlockStrategy
    {
        // Editor fields
        [SerializeField] private GameObject _objectToInstantiate;



        // Core
        protected override CurveBlock ApplyStrategy(CurveBlock block)
        {
            var t = (block.Curve.MinT + block.Curve.MaxT) / 2;
            var position = block.CurveSurface.GetTopPosition(t, 0);
            var (forward, right, up) = block.Curve.GetLocalBasis(t);
            var orientedGameObject = NewOrientedGameObject(position, forward, up);
            var gameObject = Instantiate(_objectToInstantiate, parent: orientedGameObject.transform);
            gameObject.transform.parent = block.transform;
            return block;
        }
    }
}
