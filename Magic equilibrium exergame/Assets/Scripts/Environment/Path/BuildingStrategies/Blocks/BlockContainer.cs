using Assets.Scripts.Models.Path.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Path.BuildingStrategies.Blocks
{
    public class BlockContainer : StrategyContainer<BlockStrategy>
    {
       [field:SerializeField] [field:CannotBeNull] public CurveBlock CurveBlockPrefab { get; private set; }
    }
}
