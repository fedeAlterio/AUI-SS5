using Assets.Scripts.Path.BuildingStrategies.Path;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Menu
{
    [Serializable]
    public sealed class MyConfig : GameConfiguration, IPathConfiguration
    {
        // IPathConfiguration
        [PropertyRange(1, 3)]
        [PropertyDefaultValue(2)]
        public int Difficulty { get; set; }


        [PropertyRange(1, 200)]
        public int Length { get; set; }


        [Hidden]
        public float PathThickness { get; set; } = 6;

        [Hidden]
        public float CurveSize { get; set; } = 10;

        [Hidden]
        public float TextureScale { get; set; } = 0.25f;

        [Hidden]
        public float PathHeight { get; set; } = 1;


        // Path strategies
        private bool _slalom;

        [BindToStrategy(typeof(SlalomPath))]
        public bool Slalom { get; set; }

        
        [BindToStrategy(typeof(UpRampPathStrategy))]
        public bool UpRamp { get; set; }


        [BindToStrategy(typeof(DownRampPathStrategy))]
        public bool DownRamp { get; set; }                


        [BindToStrategy(typeof(UpRampPathStrategy))]
        public bool Holes { get; set; }


        [BindToStrategy(typeof(MovingPlatformPathStrategy))]
        public bool MovingPlatform { get; set; }


        [BindToStrategy(typeof(CoinsPathStrategy))]
        public bool Coins { get; set; }












        // TODO performance optimization
        public IReadOnlyList<string> PathStrategiesAllowed
        {
            get
            {
                var strategiesWithProperties = from property in GetType().GetProperties()
                                               let bindingAttribute = property.GetCustomAttributes(typeof(BindToStrategyAttribute), true)
                                                                              .Cast<BindToStrategyAttribute>()
                                                                              .FirstOrDefault()
                                               where bindingAttribute != null
                                               let isStrategyEnabled = (bool) property.GetValue(this)
                                               where isStrategyEnabled
                                               select new { bindingAttribute, property };
                var allowedStrategiesNames = strategiesWithProperties.Select(x => x.bindingAttribute.Type.Name).ToList();
                return allowedStrategiesNames;
            }
        }



        // Utils
        public override string ToString()
        {
            return nameof(MyConfig);
        }
    }
}