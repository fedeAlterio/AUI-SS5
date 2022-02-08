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
        private Dictionary<Type, Func<object>> _dependencies = new Dictionary<Type, Func<object>>();        



        // Initialization
        private void Awake()
        {
            Instance = this;
            RegisterDependencies();
            InitializeDefaults(); 
        }

        private void RegisterDependencies()
        {
            IPathConfiguration configuration = GameSetting.instance?.configuration as IPathConfiguration;
            if (configuration != null)
                Register<IPathConfiguration>(() => configuration);
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
        public void Register<T> (Func<T> builder)
        {
            _ = builder ?? throw new ArgumentNullException(nameof(builder));
            _dependencies[typeof(T)] = () => builder();
        }
        public T GetInstance<T>() where T : class
        {
            var type = typeof(T);
            if(_dependencies.TryGetValue(type, out var instanceBuilder))
                return (T) instanceBuilder.Invoke();

            object instance = this.GetInstances<T>().FirstOrDefault();
            if (instance != null)
                return (T) instance;

            if (_defaults.TryGetValue(type, out instance))
                return (T) instance;

            throw new InvalidOperationException($"Type {type} not registered");
        }
    }
}
