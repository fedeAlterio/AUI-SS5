using Assets.Scripts.Models.Path.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Path.BuildingStrategies.Blocks
{
    public interface IBlockStrategy
    {
        CurveBlock Strategy(CurveBlock block);
        public string Name { get; }
    }

}

