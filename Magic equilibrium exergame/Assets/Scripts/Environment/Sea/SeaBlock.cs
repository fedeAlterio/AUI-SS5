using Assets.Scripts.Models;
using Assets.Scripts.Models.Path.Generation.Surface;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Assets.Scripts.Models.Path.Generation.Surface.DiscreteSurface;

namespace Assets.Scripts.Sea
{
    public class SeaBlock : MonoBehaviour
    {
        // Editor fields
        [SerializeField] private float _closeDistance;



        // Private fields
        Transform _targetTransform;
        private MeshFilter _meshFilter;
        private MeshData _currentMeshData;
        private Mesh _closeMesh;
        private Mesh _farMesh;



        // Mesh data
        private static DiscreteSurface _farSurface;
        private static MeshData _farMeshData;
        private static DiscreteSurface _closeSurface;
        private static MeshData _closeMeshData;



        // Initialization
        private void Awake()
        {
            _targetTransform = GameObject.FindGameObjectWithTag(UnityTag.Player).transform;
            _meshFilter = GetComponent<MeshFilter>();
        }


        private void Start()
        {
            _closeMesh = _closeSurface.BuildMesh(_closeMeshData);
            _farMesh = _farSurface.BuildMesh(_farMeshData);
        }


        // Properties
        public bool IsPlayerClose
        {
            get
            {
                
                var seaProjectionTargetPosition = new Vector3(_targetTransform.position.x, _meshFilter.mesh.bounds.center.y, _targetTransform.position.z);                
                seaProjectionTargetPosition = transform.InverseTransformPoint(seaProjectionTargetPosition);
                return _meshFilter.mesh.bounds.SqrDistance(seaProjectionTargetPosition) < _closeDistance;
            }
        }        



        // Public
        public static void InitializeBlocks(int farVertexCount, int closeVertexCount)
        {
            (_farSurface, _farMeshData) = BuildMeshData(farVertexCount);
            (_closeSurface, _closeMeshData) = BuildMeshData(closeVertexCount);
        }



        // Events
        private void Update()
        {           
            var isPlayerClose = IsPlayerClose;
            var desiredMeshData = isPlayerClose ? _closeMeshData : _farMeshData;
            if (desiredMeshData == _currentMeshData)
                return;

            _currentMeshData = desiredMeshData;
            _meshFilter.mesh = isPlayerClose ? _closeMesh : _farMesh;
        }






        // Core
        private static (DiscreteSurface, MeshData) BuildMeshData(int vertexCount)
        {
            var plane = Surfaces.Plane();
            var discretePlane = new DiscreteSurface(plane)
            {
                UVertexCount = vertexCount,
                VVertexCount = vertexCount
            };
            return (discretePlane, discretePlane.BuildMeshData());
        }
    }
}
