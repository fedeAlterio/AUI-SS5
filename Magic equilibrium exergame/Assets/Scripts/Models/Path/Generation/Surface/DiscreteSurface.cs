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
        private PiercedSurface Surface { get; set; }
        public int UVertexCount { get; set; } = 20;
        public int VVertexCount { get; set; } = 20;        



        // Public 
        public Mesh BuildMesh()
        {
            var vertices = new List<Vector3>();
            var indices = new List<int>();
            var normals = new List<Vector3>();

            var du = (Surface.UMax - Surface.UMin) / (UVertexCount - 1);
            var dv = (Surface.VMax - Surface.VMin) / (VVertexCount - 1);

            void AddIndex(int uIndex, int vIndex)
            {
                var index = uIndex * VVertexCount + vIndex;
                indices.Add(index);
                if (index >= vertices.Count)
                {
                    Debug.Log(uIndex);
                    Debug.Log(UVertexCount);
                    throw new InvalidOperationException($"Strange");
                }
            }


            for(var i=0; i < UVertexCount; i++)
                for(var j=0; j < VVertexCount; j++)
                {
                    var (u, v) = (Surface.UMin + i * du, Surface.VMin + j * dv);
                    Surface.TryGetPointAt(u, v, out var vertex);
                    vertices.Add(vertex);
                }


            for (var i = 0; i + 1 < UVertexCount; i++)
                for (var j = 0; j + 1 < VVertexCount; j++)
                {
                    var u1 = Surface.UMin + i * du;
                    var v1 = Surface.VMin + j * dv;

                    var u2 = i + 1 == UVertexCount ? Surface.UMax : u1 + du;
                    var v2 = j + 1 == VVertexCount ? Surface.VMax : v1 + dv;

                    var allValidVertices = Surface.TryGetPointAt(u1, v1, out _)
                            && Surface.TryGetPointAt(u1, v2, out _)
                            && Surface.TryGetPointAt(u2, v1, out _)
                            && Surface.TryGetPointAt(u2, v2, out _);

                    if(allValidVertices)
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

            var mesh = new Mesh
            {
                vertices = vertices.ToArray(),
                triangles = indices.ToArray(),
                normals = normals.ToArray()  
            };
            mesh.RecalculateNormals();
            return mesh;
        }
    }
}

