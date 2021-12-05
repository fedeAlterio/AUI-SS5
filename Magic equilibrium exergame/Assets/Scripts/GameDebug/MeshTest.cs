using Assets.Scripts.Models.Path.Generation;
using Assets.Scripts.Models.Path.Generation.Line;
using Assets.Scripts.Models.Path.Generation.Surface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.GameDebug
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(MeshFilter))]
    public class MeshTest : MonoBehaviour
    {
        [SerializeField] public int VertexCount;
        private int _oldVertexCount;


        private void Start()
        {
                     
        }


        private void Update()
        {
            if(_oldVertexCount != VertexCount)
            {
                _oldVertexCount = VertexCount;
                BuildMesh();
            }    
        }


        public void BuildMesh()
        {
            var meshFilter = GetComponent<MeshFilter>();
            var plane = Surfaces.Plane();
            var discretePlane = new DiscreteSurface(plane);
            discretePlane.UVertexCount = VertexCount;
            discretePlane.VVertexCount = VertexCount;
            meshFilter.mesh = discretePlane.BuildMesh();
        }
    }
}
