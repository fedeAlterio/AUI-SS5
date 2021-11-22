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
        public DiscreteSurface(ParametricSurface surface)
        {
            Surface = surface;
        }



        // Properties
        public ParametricSurface Surface { get; }
        public int UVertexCount { get; set; } = 20;
        public int VVertexCount { get; set; } = 20;



        // Public 
        public Mesh BuildMesh()
        {
            var vertices = new List<Vector3>();
            var indices = new List<int>();
            var normals = new List<Vector3>();


            var du = (Surface.UMax - Surface.UMin) / UVertexCount;
            var dv = (Surface.VMax - Surface.VMin) / VVertexCount;

            for (var i = 0; i + 1 <= UVertexCount; i++)
                for (var j = 0; j + 1 <= VVertexCount; j++)
                {
                    var u1 = Surface.UMin + i * du;
                    var v1 = Surface.VMin + j * dv;

                    var u2 = i + 1 == UVertexCount ? Surface.UMax : u1 + du;
                    var v2 = j + 1 == VVertexCount ? Surface.VMax : v1 + dv;

                    var vert1 = Surface.PointAt(u1, v1);
                    var vert2 = Surface.PointAt(u1, v2);
                    var vert3 = Surface.PointAt(u2, v1);
                    var vert4 = Surface.PointAt(u2, v2);

                    var triangleIndex = vertices.Count;

                    vertices.Add(vert1);
                    vertices.Add(vert2);
                    vertices.Add(vert3);
                    vertices.Add(vert4);


                    // normals
                    var normal1 = Surface.NormalAt(u1, v1);
                    var normal2 = Surface.NormalAt(u1, v2);
                    var normal3 = Surface.NormalAt(u2, v1);
                    var normal4 = Surface.NormalAt(u2, v2);


                    normals.Add(normal1);
                    normals.Add(normal2);
                    normals.Add(normal3);
                    normals.Add(normal4);



                    // Front triangle 1
                    indices.Add(triangleIndex + 2);
                    indices.Add(triangleIndex + 1);
                    indices.Add(triangleIndex);

                    // Front triangle 2
                    indices.Add(triangleIndex + 2);
                    indices.Add(triangleIndex + 3);
                    indices.Add(triangleIndex + 1);
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

