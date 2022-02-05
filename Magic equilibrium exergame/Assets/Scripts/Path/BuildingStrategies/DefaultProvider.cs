using Assets.Scripts.DepndencyInjection;
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
        public static IEnumerable<T> GetInstances<T>(this MonoBehaviour @this) where T : class
        {
            return DependencyInjector.Instance.GetInstances<T>();
        }

        public static T GetInstance<T>(this MonoBehaviour @this) where T : class
        {
            return DependencyInjector.Instance.GetInstance<T>();
        }
    }
}

