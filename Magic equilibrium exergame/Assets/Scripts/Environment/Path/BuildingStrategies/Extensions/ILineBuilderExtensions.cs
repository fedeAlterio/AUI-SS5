using Assets.Scripts.Models.Path.Blocks;
using Assets.Scripts.Models.Path.Generation.Line;
using Assets.Scripts.Path.BuildingStrategies.Blocks;
using Assets.Scripts.Path.BuildingStrategies.Path;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Path.BuildingStrategies.Extensions
{
    public static class ILineBuilderExtensions
    {
        public static ILineBuilder<CurveBlock> With(this ILineBuilder<CurveBlock> @this, BlockStrategy blockStrategy)
            => @this.With(blockStrategy.Strategy);
        public static ILineBuilder<CurveBlock> GoWith(this ILineBuilder<CurveBlock> @this, IPathStrategy pathStrategy, IPathConfiguration pathConfiguration)
            => pathStrategy.Build(@this, pathConfiguration);
        public static ILineBuilder<CurveBlock> GoWith(this ILineBuilder<CurveBlock> @this, PathStrategy pathStrategy)
            => pathStrategy.Build(@this);
    }
}
