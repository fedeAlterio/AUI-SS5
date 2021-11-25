using Assets.Scripts.Models.Path.Generation.Line;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Models.Path.Generation.Surface
{
    public class CurveSurface : DiscreteSurface
    {
        public CurveSurface(DiscreteCurve curve, float thickness) : base(Surfaces.FromCurve(curve.Curve, thickness))
        {
            Curve = curve.Curve;
            Thickness = thickness;
            UVertexCount = curve.VertexCount;
            VVertexCount = 3;
        }

        public CurveSurface(PiercedSurface curveSurface, ParametricCurve curve, float thickness) : base(curveSurface)
        {
            Curve = curve;
            Thickness = thickness;
        }



        // Properties
        public ParametricCurve Curve { get; }
        public float Thickness { get; }
        public float TextureScaleFactor { get; set; } = 0.25f;
        public float Height { get; set; } = 1;



        // Mesh Building
        protected override Vector2[] BuildUvs()
        {
            var uvs = new List<Vector2>();
            Surface.TryGetPointAt(Surface.UMin, Surface.VMin, out var currentPoint);
            var totDistance = 0f;
            for (var i = 0; i < UVertexCount; i++)
            {
                if (i > 0)
                {
                    var u = UFromIndex(i);
                    Surface.TryGetPointAt(u, 0.5f * (Surface.VMin + Surface.UMax), out var newPoint);
                    totDistance += Vector3.Distance(newPoint, currentPoint);
                    currentPoint = newPoint;
                }
                var textureU = totDistance * TextureScaleFactor;
                for (var j = 0; j < VVertexCount; j++)
                {
                    var v = VFromIndex(j);
                    var textureV = (v - Surface.VMin) / Surface.VLength;
                    uvs.Add(new Vector2(textureU, textureV));
                }
            }

            return uvs.ToArray();
        }


        // Indices
        protected int[] BuildInsaddices()
        {
            var topIndices = base.BuildIndices();
            var indices = new int[topIndices.Length * 2];


            // Top face
            var i = 0;
            foreach (var index in topIndices)
            {

                indices[i] = index;
                indices[i + topIndices.Length] = index + base.TotVertices;
                i++;
            }

            return indices;
        }


        // Indices
        protected override int[] BuildIndices()
        {
            var bottomIndices = base.BuildIndices();
            var indices = new List<int>();
            indices.AddRange(Enumerable.Range(0, bottomIndices.Length * 2));


            // Top & bottom face
            {
                var i = 0;
                foreach (var index in bottomIndices)
                {
                    indices[i] = index;
                    indices[i + bottomIndices.Length] = index + base.TotVertices;
                    i++;
                }
                for (i = 0; i < bottomIndices.Length; i += 3)
                    (indices[i], indices[i + 2]) = (indices[i + 2], indices[i]);
            }


            // Right Faces

            var skipCount = 0;
            var bottomRightIndices = RightBordersIndices(skipCount).ToList();
            while (bottomRightIndices.Any())
            {
                for (var j = 0; j + 1 < bottomRightIndices.Count; j++)
                {
                    var (b1, b2) = (bottomRightIndices[j], bottomRightIndices[j + 1]);
                    var (t1, t2) = (b1 + base.TotVertices, b2 + base.TotVertices);
                    indices.AddRange(new[] { b1, t1, b2 });
                    indices.AddRange(new[] { b2, t1, t2 });
                }
                skipCount++;
                bottomRightIndices = RightBordersIndices(skipCount).ToList();
            }



            // Left Faces

            skipCount = 0;
            var bottomLeftIndices = LeftBordersIndices(skipCount).ToList();
            while (bottomLeftIndices.Any())
            {
                for (var j = 0; j + 1 < bottomLeftIndices.Count; j++)
                {
                    var (b1, b2) = (bottomLeftIndices[j], bottomLeftIndices[j + 1]);
                    var (t1, t2) = (b1 + base.TotVertices, b2 + base.TotVertices);
                    indices.AddRange(new[] { b2, t1, b1 });
                    indices.AddRange(new[] { t2, t1, b2 });
                }
                skipCount++;
                bottomLeftIndices = LeftBordersIndices(skipCount).ToList();
            }


            return indices.ToArray();
        }

        private IEnumerable<int> RightBordersIndices(int skipHoles)
        {
            for(var i=0; i < UVertexCount; i++)
            {
                bool walkingAnHole = true;
                var holesSkipped = 0;
                int? jIndex = null;
                for(var j=0; j < VVertexCount && jIndex == null; j++)
                {
                    var (u, v) = UVFromIndices(i, j);
                    var isHole = !Surface.TryGetPointAt(u, v, out var _);
                    if (isHole && !walkingAnHole)
                        if (skipHoles == holesSkipped && j >= 1)
                            jIndex = j - 1;
                        else
                            holesSkipped++;
                    walkingAnHole = isHole;
                }
                jIndex ??= walkingAnHole || holesSkipped != skipHoles ?
                    (int?) null
                    : VVertexCount - 1;
                if (jIndex != null)
                    yield return jIndex.Value + i * VVertexCount;
            }
        }


        private IEnumerable<int> LeftBordersIndices(int skipHoles)
        {
            for (var i = 0; i < UVertexCount; i++)
            {
                bool walkingAnHole = true;
                var holesSkipped = 0;
                int? jIndex = null;
                for (var j = VVertexCount - 1; j >= 0 && jIndex == null; j--)
                {
                    var (u, v) = UVFromIndices(i, j);
                    var isHole = !Surface.TryGetPointAt(u, v, out var _);
                    if (isHole && !walkingAnHole)
                        if (skipHoles == holesSkipped && j >= 1)
                            jIndex = j + 1;
                        else
                            holesSkipped++;
                    walkingAnHole = isHole;
                }
                jIndex ??= walkingAnHole || holesSkipped != skipHoles ?
                    (int?)null
                    : 0;
                if (jIndex != null)
                    yield return jIndex.Value + i * VVertexCount;
            }
        }



        // Vertices
        protected override Vector3[] BuildVertices()
    {
        var bottom = BuildBottomFaceVertices();
        var top = BuildTopFaceVertices(bottom);
        return Enumerable.Union(bottom, top).ToArray();
    }



    private Vector3[] BuildTopFaceVertices(Vector3[] topFaceVertices)
    {
        var vertices = new Vector3[topFaceVertices.Length];
        var vertexIndex = 0;
        foreach (var (i, j) in VertexIndices())
        {
            // Not properly correct but it works if there are not curves that go straight up.
            vertices[vertexIndex] = topFaceVertices[vertexIndex] + Vector3.up * Height;
            vertexIndex++;
        }

        return vertices;
    }


    private Vector3[] BuildBottomFaceVertices()
    {
        return base.BuildVertices();
    }
}
}
