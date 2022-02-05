using Assets.Scripts.Models;
using Assets.Scripts.Models.Path.Generation.Line;
using Assets.Scripts.Path.BuildingStrategies;
using Assets.Scripts.Path.BuildingStrategies.Path;
using Assets.Scripts.Path.Generation;
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
        private void Start()
        {
            _pathGenerator = FindObjectOfType<PathGenerator>();
            _pathGenerator.PathGenerated += OnPathGenerated;
            PathConfiguration = this.GetInstance<IPathConfiguration>();
        }        



        // Properties
        private IPathConfiguration PathConfiguration { get; set; }



        // Events
        private void OnPathGenerated(ParametricCurve path)
        {
            AddLightsOnPath(path);
            AddLightsOnSea();
        }






        // Core
        private void AddLightsOnSea()
        {
            
        }


        private void AddLightsOnPath(ParametricCurve path)
        {
            ClearLights();
            if (_lightsParent == null)
                _lightsParent = transform;

            var deltaX = PathConfiguration.PathThickness / 2;
            foreach (var t in path.SpaceQuantizedDomain(deltaSpace: _lightsDistance))
            {
                var (z, x, y) = path.GetLocalBasis(t);
                var top = path.PointAt(t) + y * PathConfiguration.PathHeight + _lightsHeight * y; ;
                AddLight(top);
                for (var i = 1; i < 2; i++)
                {
                    AddLight(top - (top.y - 5) * Vector3.up - x * i * 15);
                    AddLight(top - (top.y - 5) * Vector3.up + x * i * 15);
                }
            }
        }


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
