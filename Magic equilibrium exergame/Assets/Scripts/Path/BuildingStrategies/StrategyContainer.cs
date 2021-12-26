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
            Strategies = _strategies.ToDictionary(x => x.GetType().Name, x => x);
        }

        public void SearchStrategies()
        {
            _strategies = this.FindInstances<T>().ToList();
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
            return Strategies.TryGetValue(typeof(V).Name, out var pathBlock)
                ? pathBlock as V
                : throw new InvalidOperationException($"Path block of type {typeof(V)} not found");
        }
    }
}
