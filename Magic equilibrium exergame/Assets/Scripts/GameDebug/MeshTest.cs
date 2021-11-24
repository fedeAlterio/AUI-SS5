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


        private void Start()
        {
            var meshFilter = GetComponent<MeshFilter>();
            meshFilter.mesh = new DiscreteSurface(Surfaces.PlaneWithHole()) { UVertexCount = 50, VVertexCount = 50 }.BuildMesh();
        }


        private void Update()
        {
            
        }
    }
}
