using Assets.Scripts.Models.Path.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Path.BuildingStrategies.Path
{
    public interface IPathStrategyContainer
    {
        IReadOnlyDictionary<Type, IPathStrategy> Strategies { get; }
        T GetStrategy<T>() where T : class, IPathStrategy;
    }
}

