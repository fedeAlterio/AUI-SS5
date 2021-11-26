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
        // Private fields
        private List<Vector3> _vertices = new List<Vector3>();
        private List<int> _indices = new List<int>();


        
        // Initialization
        public CurveSurface(DiscreteCurve curve, float thickness, float height) : base(Surfaces.FromCurve(curve.Curve, thickness))
        {
            Height = height; 
            Curve = curve.Curve;
            Thickness = thickness;
            UVertexCount = curve.VertexCount;
            VVertexCount = 3;
        }

        public CurveSurface(PiercedSurface curveSurface, ParametricCurve curve, float thickness, float height) : base(curveSurface)
        {
            Height = height;
            Curve = curve;
            Thickness = thickness;
        }



        // Properties
        public ParametricCurve Curve { get; }
        public float Thickness { get; }
        public float TextureScaleFactor { get; set; } = 0.25f;
        public float Height { get; set; } = 0.1f;



        // Mesh Building
        public override Mesh BuildMesh()
        {
            BuildBottomFace();
            BuildTopFace();
            BuildLeftFace();
            BuildRightFace();
            BuildBackFace();
            BuildFrontFace();
            return base.BuildMesh();
        }

        protected override Vector3[] BuildVertices()
        {
            return _vertices.ToArray();
        }

        protected override int[] BuildIndices()
        {
            return _indices.ToArray();
        }


        // Bottom face
        private void BuildBottomFace()
        {
            var vertices = base.BuildVertices();
            _vertices.AddRange(vertices);
            _indices.AddRange(base.BuildIndices());
            for (var i = 0; i < _indices.Count; i += 3)
                (_indices[i], _indices[i + 2]) = (_indices[i + 2], _indices[i]);
        }


        // Top face
        private void BuildTopFace()
        {            
            var totVertices = _vertices.Count;
            for(var i=0; i < totVertices; i++)
                _vertices.Add(_vertices[i] + Vector3.up * Height);            

            var totIndices = _indices.Count;
            for(int i=0; i < totIndices; i++)
                _indices.Add(_indices[i] + totVertices);
            for (var i = totIndices; i < _indices.Count; i += 3)
                (_indices[i], _indices[i + 2]) = (_indices[i + 2], _indices[i]);
        }


        // Left Face
        private void AddTriangle(int index1, int index2, int index3)
        {
            var vertices = new[] {index1, index2, index3}.Select(i => _vertices[i]);
            _vertices.AddRange(vertices);
            _indices.AddRange(new[] {_vertices.Count - 1, _vertices.Count - 2, _vertices.Count - 3});
        }

        private void BuildLeftFace()
        {
            var skipCount = 0;
            var leftIndices = LeftBordersIndices(skipCount).ToList();
            while (leftIndices.Any())
            {
                for (var j = 0; j + 1 < leftIndices.Count; j++)
                {
                    var (b1, b2) = (leftIndices[j], leftIndices[j + 1]);
                    var (t1, t2) = (b1 + base.TotVertices, b2 + base.TotVertices);
                    AddTriangle(b1, t1, b2);
                    AddTriangle(b2, t1, t2);
                }
                skipCount++;
                leftIndices = LeftBordersIndices(skipCount).ToList();
            }
        }

        private IEnumerable<int> LeftBordersIndices(int holesToSkip)
        {
            for (var i = 0; i < UVertexCount; i++)
            {
                bool walkingInAHole = true;
                var holesSkipped = 0;
                int? jIndex = null;
                for (var j = VVertexCount - 1; j >= 0 && jIndex == null; j--)
                {
                    var (u, v) = UVFromIndices(i, j);
                    var isHole = !Surface.TryGetPointAt(u, v, out var _);
                    if (isHole && !walkingInAHole)
                        if (holesToSkip == holesSkipped && j >= 1)
                            jIndex = j + 1;
                        else
                            holesSkipped++;
                    walkingInAHole = isHole;
                }
                jIndex ??= walkingInAHole || holesSkipped != holesToSkip ?
                    (int?)null
                    : 0;
                if (jIndex != null)
                    yield return jIndex.Value + i * VVertexCount;
            }
        }



        // Right
        private void BuildRightFace()
        {
            var skipCount = 0;
            var rightIndices = RightBordersIndices(skipCount).ToList();
            while (rightIndices.Any())
            {
                for (var j = 0; j + 1 < rightIndices.Count; j++)
                {
                    var (b1, b2) = (rightIndices[j], rightIndices[j + 1]);
                    var (t1, t2) = (b1 + base.TotVertices, b2 + base.TotVertices);
                   AddTriangle(b2, t1, b1);
                   AddTriangle(t2, t1, b2);
                }
                skipCount++;
                rightIndices = RightBordersIndices(skipCount).ToList();
            }
        }

        private IEnumerable<int> RightBordersIndices(int holesToSkip)
        {
            for (var i = 0; i < UVertexCount; i++)
            {
                bool walkingInAHole = true;
                var holesSkipped = 0;
                int? jIndex = null;
                for (var j = 0; j < VVertexCount && jIndex == null; j++)
                {
                    var (u, v) = UVFromIndices(i, j);
                    var isHole = !Surface.TryGetPointAt(u, v, out var _);
                    if (isHole && !walkingInAHole)
                        if (holesToSkip == holesSkipped && j >= 1)
                            jIndex = j - 1;
                        else
                            holesSkipped++;
                    walkingInAHole = isHole;
                }
                jIndex ??= walkingInAHole || holesSkipped != holesToSkip ?
                    (int?)null
                    : VVertexCount - 1;
                if (jIndex != null)
                    yield return jIndex.Value + i * VVertexCount;
            }
        }



        // Back face
        private void BuildBackFace()
        {
            var skipCount = 0;
            var backIndices = BackBorderIndices(skipCount).ToList();
            while (backIndices.Any())
            {
                for (var j = 0; j + 1 < backIndices.Count; j++)
                {
                    var (b1, b2) = (backIndices[j], backIndices[j + 1]);
                    var (t1, t2) = (b1 + base.TotVertices, b2 + base.TotVertices);
                    AddTriangle(b2, t1, b1);
                    AddTriangle(t2, t1, b2);
                }
                skipCount++;
                backIndices = BackBorderIndices(skipCount).ToList();
            }
        }

        private IEnumerable<int> BackBorderIndices(int holesToSkip)
        {
            var walkingInAHole = true;
            var i = 0;
            for (var j = 0; j < VVertexCount; j++)
                if (holesToSkip < 0)
                    break;
                else
                {
                    var (u, v) = UVFromIndices(i, j);
                    var isAHole = !Surface.TryGetPointAt(u, v, out var _);
                    if (!isAHole && holesToSkip == 0)
                        yield return i * VVertexCount + j;

                    if (isAHole && !walkingInAHole)
                        holesToSkip--;
                    walkingInAHole = isAHole;
                }
        }



        // Front face
        private void BuildFrontFace()
        {
            var skipCount = 0;
            var frontIndices = FrontBorderIndices(skipCount).ToList();
            while (frontIndices.Any())
            {
                for (var j = 0; j + 1 < frontIndices.Count; j++)
                {
                    var (b1, b2) = (frontIndices[j], frontIndices[j + 1]);
                    var (t1, t2) = (b1 + base.TotVertices, b2 + base.TotVertices);
                    AddTriangle(b1, t1, b2);
                    AddTriangle(b2, t1, t2);
                }
                skipCount++;
                frontIndices = FrontBorderIndices(skipCount).ToList();
            }

        }

        private IEnumerable<int> FrontBorderIndices(int holesToSkip)
        {
            var walkingInAHole = true;
            var i = UVertexCount - 1;
            for (var j = 0; j < VVertexCount; j++)
                if (holesToSkip < 0)
                    break;
                else
                {
                    var (u, v) = UVFromIndices(i, j);
                    var isAHole = !Surface.TryGetPointAt(u, v, out var _);
                    if (!isAHole && holesToSkip == 0)
                        yield return i * VVertexCount + j;

                    if (isAHole && !walkingInAHole)
                        holesToSkip--;
                    walkingInAHole = isAHole;
                }
        }




        private static float _totDistanceTop;
        protected override Vector2[] BuildUvs()
        {
            var uvs = new List<Vector2>();
            Surface.TryGetPointAt(Surface.UMin, Surface.VMin, out var currentPoint);
            for (var i = 0; i< TotVertices; i++)
                uvs.Add(new Vector2(0, 0));

            void Addtexture()
            {                
                for (var i = 0; i < UVertexCount; i++)
                {
                    if (i > 0)
                    {
                        var u = UFromIndex(i);
                        Surface.TryGetPointAt(u, 0.5f * (Surface.VMin + Surface.UMax), out var newPoint);
                        _totDistanceTop += Vector3.Distance(newPoint, currentPoint);
                        currentPoint = newPoint;
                    }
                    var textureU = _totDistanceTop * TextureScaleFactor;
                    for (var j = 0; j < VVertexCount; j++)
                    {
                        var v = VFromIndex(j);
                        var textureV = (v - Surface.VMin) / Surface.VLength;
                        uvs.Add(new Vector2(textureU, textureV));
                    }
                }
            }

            //Addtexture();
            Addtexture();

            for (var i = 2* TotVertices; i < _vertices.Count; i++)
                uvs.Add(new Vector2(0, 0));
            return uvs.ToArray();
        }        
    }
}
