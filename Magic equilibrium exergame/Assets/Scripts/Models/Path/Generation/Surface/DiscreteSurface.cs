using Assets.Scripts.Models.Path.Generation.Line;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Models.Path.Generation.Surface
{
    public class DiscreteSurface 
    {
        // Initialization

        protected DiscreteSurface() { }
        public DiscreteSurface(PiercedSurface surface)
        {
            Surface = surface;
        }
        public DiscreteSurface(ParametricSurface surface)
            : this(PiercedSurface.FromParametricSurface(surface))
        {

        }



        // Properties
        protected PiercedSurface Surface { get; set; }
        public int UVertexCount { get; set; } = 20;
        public int VVertexCount { get; set; } = 20;
        public virtual int TotVertices => UVertexCount * VVertexCount;
        public float Du => (Surface.UMax - Surface.UMin) / (UVertexCount - 1);
        public float Dv => (Surface.VMax - Surface.VMin) / (VVertexCount - 1);


        // Building Mesh
        
        protected float UFromIndex(int i) => i == UVertexCount - 1 ? Surface.UMax : Surface.UMin + i * Du;
        protected float VFromIndex(int j) => j == VVertexCount - 1 ? Surface.VMax : Surface.VMin + j * Dv;
        protected (float u, float v) UVFromIndices(int i, int j) => (UFromIndex(i), VFromIndex(j));


        protected IEnumerable<(int i, int j)> VertexIndices()
        {
            for (var i = 0; i < UVertexCount; i++)
                for (var j = 0; j < VVertexCount; j++)
                    yield return (i, j);
        }

        protected virtual Vector3[] BuildVertices()
        {
            var vertices = new List<Vector3>();
            foreach(var (i,j) in VertexIndices())
            {
                var (u, v) = UVFromIndices(i, j);
                Surface.TryGetPointAt(u, v, out var vertex);
                vertices.Add(vertex);
            }

            return vertices.ToArray();
        }

        protected virtual Vector2[] BuildUvs()
        {
            var uvs = new List<Vector2>();
            foreach(var (i,j) in VertexIndices())
            {
                var (u, v) = UVFromIndices(i, j);
                var uv = new Vector2((u - Surface.UMin) / Surface.ULength, (v - Surface.VMin) / Surface.VLength);
                uvs.Add(uv);
            }
            return uvs.ToArray();
        }


        protected virtual int[] BuildIndices()
        {
            var indices = new List<int>();
            void AddIndex(int uIndex, int vIndex) => indices.Add(uIndex * VVertexCount + vIndex);

            for (var i = 0; i + 1 < UVertexCount; i++)
                for (var j = 0; j + 1 < VVertexCount; j++)
                {
                    var (u1, v1) = UVFromIndices(i, j);
                    var (u2, v2) = UVFromIndices(i + 1, j + 1);

                    var allValidVertices = Surface.TryGetPointAt(u1, v1, out _)
                            && Surface.TryGetPointAt(u1, v2, out _)
                            && Surface.TryGetPointAt(u2, v1, out _)
                            && Surface.TryGetPointAt(u2, v2, out _);

                    if (allValidVertices)
                    {
                        // Front triangle 1
                        AddIndex(i, j);
                        AddIndex(i + 1, j);
                        AddIndex(i + 1, j + 1);

                        // Front triangle 2
                        AddIndex(i, j);
                        AddIndex(i + 1, j + 1);
                        AddIndex(i, j + 1);
                    }
                }
            return indices.ToArray();
        }

        public virtual Mesh BuildMesh()
        {
            var mesh = new Mesh
            {
                vertices = BuildVertices(),
                triangles = BuildIndices(),
                uv = BuildUvs()
            };
            mesh.RecalculateNormals();
            return mesh;
        }
    }
}

