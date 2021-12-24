using Assets.Scripts.Models.Path.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Path.BuildingStrategies.Blocks
{
    public abstract class BlockStrategy : MonoBehaviour, IBlockStrategy
    {
        [field:SerializeField] public string Name { get; protected set; }

        public CurveBlock Strategy(CurveBlock block)
        {
            AddName(block);
            block = ApplyStrategy(block);
            return block;
        }

        protected abstract CurveBlock ApplyStrategy(CurveBlock block);


        protected void AddName(CurveBlock block) => AddName(block, Name);
        protected void AddName(CurveBlock block, string name)
        {
            block.name += $" {name}";
            block.name.Trim();
        }
    }
}

