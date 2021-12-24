using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Path.BuildingStrategies
{
    public abstract class StrategyContainer<T> : MonoBehaviour where T : MonoBehaviour
    {
        // Editor 
        [SerializeField] private bool _searchStrategies;
        [SerializeField] [CannotBeNull] protected List<T> _strategies = new List<T>();


        // Initialization
        private void Start()
        {
            Strategies = _strategies.ToDictionary(x => x.GetType(), x => x);
        }

        public void SearchStrategies()
        {
            _strategies = this.FindInstances<T>().ToList();
            _searchStrategies = false;
        }


        // Properties|
        public IReadOnlyDictionary<Type, T> Strategies { get; private set; }



        // Editor
        private void OnValidate()
        {
            SearchStrategies();
        }


        // Public
        public V Get<V>() where V : class, T
        {
            return Strategies.TryGetValue(typeof(V), out var pathBlock)
                ? pathBlock as V
                : throw new InvalidOperationException($"Path block of type {typeof(V)} not found");
        }
    }
}
