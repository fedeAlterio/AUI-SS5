using Assets.Scripts.Abstractions;
using Assets.Scripts.DepndencyInjection.Mocks;
using Assets.Scripts.Path.BuildingStrategies;
using Assets.Scripts.Path.BuildingStrategies.Path;
using Assets.Scripts.PlayerMovement;
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
        }

        private void AddDefault<T>(T builder)
        {
            _defaults.Add(typeof(T), builder);
        }
        


        // Properties
        public static DependencyInjector Instance { get; private set; }



        // Public

        public T GetInstance<T>() where T : class
        {
            var ret = this.GetInstances<T>().FirstOrDefault();
            if (ret != null)
                return ret;

            if (_defaults.TryGetValue(typeof(T), out var defaultInstance))
                return (T) defaultInstance;

            return null;
        }
    }
}
