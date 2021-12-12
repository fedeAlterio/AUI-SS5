using Assets.Scripts.Models.Path;
using Assets.Scripts.Models.Path.Generation.Line;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Environment.ObjectSpawner
{
    public class LightsSpawner : MonoBehaviour
    {
        // Editor fields
        [SerializeField] private GameObject _lightPrefab;
        [SerializeField] private Transform _lightsParent;
        [SerializeField] private int _lightsHeight;
        [SerializeField] private float _lightsDistance;



        // Private fields
        private PathGenerator _pathGenerator;
        private List<GameObject> _lights = new List<GameObject>();



        // Initialization
        private void Awake()
        {
            _pathGenerator = FindObjectOfType<PathGenerator>();
            _pathGenerator.PathGenerated += OnPathGenerated;
        }



        // Events
        private void OnPathGenerated(ParametricCurve path)
        {           
            ClearLights();
            if (_lightsParent == null)
                _lightsParent = transform;

            var deltaX = _pathGenerator.PathThickness / 2;
            foreach(var t in path.SpaceQuantizedDomain(deltaSpace: _lightsDistance))
            {
                var(z, x, y) = path.GetLocalBasis(t);
                var top = path.PointAt(t) + y * _pathGenerator.PathHeight;
                var lightL = top - x * deltaX + _lightsHeight * y;
                var lightR = top + x * deltaX + _lightsHeight * y;
                AddLight(lightL);
                AddLight(lightR);
            }
        }



        // Core
        private void ClearLights()
        {
            foreach (var light in _lights)
                Destroy(light);
            _lights.Clear();
        }

        private void AddLight(Vector3 position)
        {            
            var newLight = Instantiate(_lightPrefab, position: position, rotation: Quaternion.identity, parent: _lightsParent);
            _lights.Add(newLight);
        }
    }
}
