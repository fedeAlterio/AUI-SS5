using Assets.Scripts.Path.BuildingStrategies.Path;
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
        private void Awake()
        {
            Strategies = _strategies.ToDictionary(x => x.GetType().Name, x => x);
        }

        public void SearchStrategies()
        {
            _strategies = this.GetInstances<T>().ToList();
            Debug.Log("Searching in " + GetType());
            _searchStrategies = false;
        }



        // Properties
        public IReadOnlyDictionary<string, T> Strategies { get; private set; }        



        // Editor
        private void OnValidate()
        {
            if (!_searchStrategies)
                return;

            SearchStrategies();
        }


        // Public
        public V Get<V>() where V : class, T
        {
            return GetByName(typeof(V).Name) as V;
        }

        public T GetByName(string name)
        {
            return Strategies.TryGetValue(name, out var pathBlock)
                ? pathBlock
                : throw new InvalidOperationException($"Path block of type {typeof(T)} not found");
        }
    }
}
