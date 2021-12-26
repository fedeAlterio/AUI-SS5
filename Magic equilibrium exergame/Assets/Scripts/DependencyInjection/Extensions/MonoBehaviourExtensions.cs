using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.DependencyInjection.Extensions
{
    public static class MonoBehaviourExtensions
    {
        private static DependenciesContainer _dependenciesContainer = new DependenciesContainer();
        public static void Register<T,V>(this MonoBehaviour @this) where V : MonoBehaviour, T where T : class
        {
            @this.Register<T>(GameObject.FindObjectOfType<V>());
        }

        public static void Register<T>(this MonoBehaviour _, T instance) where T : class
        {
            _dependenciesContainer.Register<T, T>(() => instance);
        }

        public static void Register(this MonoBehaviour @this, Type type, object instance)
        {
            _dependenciesContainer.Register(type, () => instance);
        }

        public static T GetInstance<T>(this MonoBehaviour _) where T : class => _dependenciesContainer.Get<T>();        
    }
}
