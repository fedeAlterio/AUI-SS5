using Assets.Scripts.DependencyInjection.Extensions;
using Assets.Scripts.Models.Path.Generation.Line;
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
    public abstract class CurveSpawner : MonoBehaviour
    {
        // Editor fields
        [SerializeField] private List<GameObject> _objectsToSpawn;
        [SerializeField] private float _objectsDistance;
        [SerializeField] private float _horizontalOffset;
        [SerializeField] private Transform _objectsParent;
        [SerializeField] private int _objectsColumns;
        [SerializeField] private bool _useParentHeight;
        [SerializeField] private Vector3 _randomRange;



        // Fields
        private PathGenerator _pathGenerator;
        protected List<GameObject> _inGameObjects = new List<GameObject>();



        // Properties
        private IPathConfiguration PathConfiguration => this.GetInstance<IPathConfiguration>();



        // Events
        protected void GenerateObjects(ParametricCurve curve)
        {
            foreach (var obj in _inGameObjects)
                Destroy(obj);
            _inGameObjects.Clear();

            foreach (var t in curve.SpaceQuantizedDomain(deltaSpace: _objectsDistance))
            {
                var (z, x, y) = curve.GetLocalBasis(t);
                var top = curve.PointAt(t) + y * PathConfiguration.PathHeight;
                if (_useParentHeight && _objectsParent)
                    top.y = _objectsParent.transform.position.y;

                for (var i = 1; i <= _objectsColumns; i++)
                {
                    AddObject(top - x * (i * _horizontalOffset + 0.5f * _horizontalOffset * UnityEngine.Random.Range(0, 1f)));
                    AddObject(top + x * (i * _horizontalOffset + 0.5f * _horizontalOffset * UnityEngine.Random.Range(0f, 1f)));
                }
            }
        }

        private void AddObject(Vector3 position)
        {
            var randomIndex = UnityEngine.Random.Range(0, _objectsToSpawn.Count);
            position += RandomOffset(_randomRange);
            var rock = _objectsToSpawn[randomIndex];
            var wrapper = new GameObject();
            wrapper.transform.parent = _objectsParent;
            wrapper.transform.position = position;
            var newRock = Instantiate(rock, parent : wrapper.transform);
            newRock.transform.localRotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0);
            _inGameObjects.Add(wrapper);
        }

        private Vector3 RandomOffset(Vector3 range)
        {
            float Random(float length) => UnityEngine.Random.Range(-length/2, length/2);
            return new Vector3(Random(range.x), Random(range.y), Random(range.z));
        }
    }
}
