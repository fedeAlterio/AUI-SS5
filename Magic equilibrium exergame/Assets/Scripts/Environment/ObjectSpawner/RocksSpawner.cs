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
    public class RocksSpawner : MonoBehaviour
    {
        // Editor fields
        [SerializeField] private List<GameObject> _rocks;
        [SerializeField] private float _rocksDistance;
        [SerializeField] private float _horizontalOffset;
        [SerializeField] private Transform _rocksParent;
        [SerializeField] private int _rocksColumns;



        // Private fields
        private PathGenerator _pathGenerator;
        private List<GameObject> _inGameRocks = new List<GameObject>();



        // Initialization
        private void Awake()
        {
            _pathGenerator = FindObjectOfType<PathGenerator>();
            _pathGenerator.PathGenerated += OnPathGenerated;
        }




        // Events
        private void OnPathGenerated(ParametricCurve curve)
        {
            foreach (var t in curve.SpaceQuantizedDomain(deltaSpace: _rocksDistance))
            {
                var (z, x, y) = curve.GetLocalBasis(t);
                var top = curve.PointAt(t) + y * _pathGenerator.PathHeight;
                top -= top.y * Vector3.up;
                
                for(var i=1; i <=_rocksColumns; i++)
                {
                    AddRock(top - x *( i * _horizontalOffset + 0.5f * _horizontalOffset* UnityEngine.Random.Range(0,1f)));
                    AddRock(top + x * (i * _horizontalOffset + 0.5f * _horizontalOffset * UnityEngine.Random.Range(0f, 1f)));
                }
            }
        }

        private void AddRock(Vector3 position)
        {
            var randomIndex = UnityEngine.Random.Range(0, _rocks.Count);            
            var rock = _rocks[randomIndex];
            var wrapper = new GameObject();
            wrapper.transform.parent = _rocksParent;
            wrapper.transform.position = position;
            var newRock = Instantiate(rock, parent : wrapper.transform);
            newRock.transform.localRotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0);
            _inGameRocks.Add(wrapper);
        }
    }
}
