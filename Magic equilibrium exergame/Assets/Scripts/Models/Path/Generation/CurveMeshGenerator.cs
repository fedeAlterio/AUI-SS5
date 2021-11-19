using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Models.Path.Generation
{
    public class CurveMeshGenerator
    {
        public Mesh FromCurve()
        {
            //var curve = Curves.Line(Vector3.zero, new Vector3(3, 3, 3));
            var curve = Curves.Circle();
            var surface = Surfaces.FromCurve(curve, 0.3f);
            return FromSurface(surface);
        }


        public Mesh SampleTriangle()
        {
            var mesh = new Mesh
            {
                vertices = new Vector3[]
               {
                    new Vector3(0, 0, 0),
                    new Vector3(1, 0, 0),
                    new Vector3(1, 0, 1),
               },
                triangles = new int[] { 0, 1, 2, 2, 1, 0 }
            };
            mesh.RecalculateNormals();

            return mesh;
        }


        public Mesh FromSurface(ParametricSurface surface)
        {
            var vertices = new List<Vector3>();
            var indices = new List<int>();

            var steps = 20;
            var du = (surface.UMax - surface.UMin) / steps;
            var dv = (surface.VMax - surface.VMin) / steps;

            Debug.Log($"VMIn; {surface.VMin}");
            for(var i=0; i + 1 <= steps; i++)
                for(var j=0; j + 1 <= steps; j++)
                {
                    var u1 = surface.UMin + i * du;
                    var v1 = surface.VMin + j * dv;

                    var u2 = i + 1 == steps ? surface.UMax : u1 + du;
                    var v2 = j + 1 == steps ? surface.VMax : v1 + dv;


                    var vert1 = surface.PointAt(u1, v1);
                    var vert2 = surface.PointAt(u1, v2);
                    var vert3 = surface.PointAt(u2, v1);
                    var vert4 = surface.PointAt(u2, v2);

                    if (new[] { vert1, vert2, vert3, vert4 }.Distinct().Count() != 4)
                        Debug.Log("Grteve");

                    vert1 = surface.PointAt(u1, v1);
                    vert2 = surface.PointAt(u1, v2);
                    vert3 = surface.PointAt(u2, v1);
                    vert4 = surface.PointAt(u2, v2);


                    var triangleIndex = vertices.Count;

                    vertices.Add(vert1);
                    vertices.Add(vert2);
                    vertices.Add(vert3);
                    vertices.Add(vert4);

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
                triangles = indices.ToArray()
            };            
            mesh.RecalculateNormals();
            return mesh;
        }
    }
}
