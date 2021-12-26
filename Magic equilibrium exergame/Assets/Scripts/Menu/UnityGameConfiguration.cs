using Assets.Scripts.Path.BuildingStrategies.Path;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Menu
{
    [Serializable]
    public class MyConfig : GameConfiguration, IPathConfiguration
    {
        // Properties

        [PropertyRename("Length")]
        public int PathLength { get; set; }


        [PropertyRange(1, 3)]
        [PropertyDefaultValue(2)]
        public int Difficulty { get; set; }


        [BindedToStrategy(typeof(SlalomPath))]
        public bool Slalom { get; set; }


        [PropertyRange(1, 20)]
        public int Length { get; set; }


        // TODO performance optimization
        public IReadOnlyList<string> PathStrategiesAllowed
        {
            get
            {
                var strategiesWithProperties = from property in GetType().GetProperties()
                                               let bindingAttribute = property.GetCustomAttributes(typeof(BindedToStrategyAttribute), true)
                                                                              .Cast<BindedToStrategyAttribute>()
                                                                              .FirstOrDefault()
                                               where bindingAttribute != null
                                               let isStrategyEnabled = (bool) property.GetValue(this)
                                               where isStrategyEnabled
                                               select new { bindingAttribute, property };
                return strategiesWithProperties.Select(x => x.bindingAttribute.Type.Name).ToList();
            }
        }

        public float PathThickness { get; set; }
        public float CurveSize { get; set; }
        public float TextureScale { get; set; }
        public float PathHeight { get; set; }



        // Utils
        public override string ToString()
        {
            return nameof(MyConfig);
        }
    }
}