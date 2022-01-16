using Assets.Scripts.Abstractions;
using Assets.Scripts.Communication;
using Assets.Scripts.Communication.Udp;
using Assets.Scripts.DependencyInjection.Extensions;
using Assets.Scripts.Menu;
using Assets.Scripts.Mock;
using Assets.Scripts.Path.BuildingStrategies;
using Assets.Scripts.Path.BuildingStrategies.Configuration;
using Assets.Scripts.Path.BuildingStrategies.Path;
using Assets.Scripts.PlayerMovement.Smoothing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.DependencyInjection
{
    public class DependenciesInjector : MonoBehaviour
    {
        // Initialization
        private void Awake()
        {
            RegisterDependencies();
        }

        private void RegisterDependencies()
        {
            var pathConfiguration = FindPathConfiguration();
            Register(() => pathConfiguration);
                                    
            Register<IWobbleboardService, UdpWobbleboardService>(defaultBuilder : () => new MockWobbleboardService());            
        }


        private IPathConfiguration FindPathConfiguration()
            => MyConfig.Instance ?? DefaultPathConfiguration();



        // Utils
        private void Register<T>(Func<T> defaultBuilder) where T : class
        {
            Register<T, T>(defaultBuilder);
        }


        private void Register<T, V>(Func<T> defaultBuilder) where T : class where V : class, T
        {
            var instance = this.FindInstances<V>().FirstOrDefault() ?? defaultBuilder.Invoke();
            this.Register(instance);
        }

        private IPathConfiguration DefaultPathConfiguration()
        {
            var strategiesNames = this.FindInstances<IPathStrategy>().Select(s => s.Name).ToList();
            return new DefaultPathConfiguration(strategiesNames);
        }
    }
}
