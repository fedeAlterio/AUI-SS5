using Assets.Scripts.DependencyInjection.Extensions;
using Assets.Scripts.Menu;
using Assets.Scripts.Path.BuildingStrategies;
using Assets.Scripts.Path.BuildingStrategies.Configuration;
using Assets.Scripts.Path.BuildingStrategies.Path;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.DependencyInjection
{
    public class DependenciesInjecter : MonoBehaviour
    {
        // Initialization
        private void Awake()
        {
            RegisterDependencies();
        }

        private void RegisterDependencies()
        {
            Register<IPathConfiguration>(defaultBuilder: DefaultPathConfiguration);
        }



        // Utils
        private void Register<T>(Func<T> defaultBuilder) where T : class
        {
            var instance = this.FindInstances<T>().FirstOrDefault() ?? defaultBuilder.Invoke();
            this.Register(instance);
        }

        private DefaultPathConfiguration DefaultPathConfiguration()
        {
            var strategiesNames = this.FindInstances<IPathStrategy>().Select(s => s.Name).ToList();
            return new DefaultPathConfiguration(strategiesNames);
        }
    }
}
