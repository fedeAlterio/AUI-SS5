using Assets.Scripts.Abstractions;
using Assets.Scripts.DepndencyInjection.Mocks;
using Assets.Scripts.Path.BuildingStrategies;
using Assets.Scripts.Path.BuildingStrategies.Path;
using Assets.Scripts.PlayerMovement;
using Assets.Scripts.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.DepndencyInjection
{
    public class DependencyInjector : MonoBehaviour
    {
        private Dictionary<Type, object> _defaults = new Dictionary<Type, object>();        
        private static Dictionary<Type, object> _staticDependencies = new Dictionary<Type, object>();



        // Initialization
        private void Awake()
        {
            Instance = this;
            InitializeDefaults(); 
        }

        private void InitializeDefaults()
        {            
            AddDefault<IPathConfiguration>(new DefaultPathConfiguration(this.GetInstances<IPathStrategy>().Select(s => s.Name).ToList()));
            AddDefault<IWobbleBoardConfiguration>(new MockWobbleBoardConfiguration());
            AddDefault<IMovementAxis>(new WASDMovementAxis());
            AddDefault<ILevelStatistics>(new LevelStatistics());    
        }

        private void AddDefault<T>(T builder)
        {
            _defaults.Add(typeof(T), builder);
        }
        


        // Properties
        public static DependencyInjector Instance { get; private set; }



        // Public
        public void RegisterStaticInstance<T>(T instance)
        {
            _staticDependencies.Add(instance.GetType(), instance);
        }


        public T GetInstance<T>() where T : class
        {
            if (_staticDependencies.TryGetValue(typeof(T), out var instance))
                return (T) instance;

            instance = this.GetInstances<T>().FirstOrDefault();
            if (instance != null)
                return (T) instance;

            if (_defaults.TryGetValue(typeof(T), out instance))
                return (T) instance;

            return null;
        }
    }
}
