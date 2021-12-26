using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Path.BuildingStrategies
{
    public static class MonobehaviourExtensions
    {
        // Building Strategies
        public static IEnumerable<T> FindInstances<T>(this MonoBehaviour @this) where T : class
        {
            var type = typeof(T);
            var instances = from t in type.Assembly.ExportedTypes
                            where type.IsAssignableFrom(t) && typeof(MonoBehaviour).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface
                            let instance = GameObject.FindObjectOfType(t) as T
                            where instance != null
                            select instance;
            return instances;
        }
    }
}

