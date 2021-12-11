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
        private List<Vector3> _normals = new List<Vector3>();



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



        // Public Methods
        public Vector3 GetTopPosition(float t, float n, float topOffset = 0)
        {
            Surface.TryGetPointAt(t, n, out var point);
            var tangent = Curve.TangentAt(Curve.MinT);
            var right = Curve.NormalAt(Curve.MinT);
            var top = Vector3.Cross(tangent, right);

            return point + (1f * Height + topOffset) * top;
        }



        // Mesh Building
        public override MeshData BuildMeshData()
        {
            ComputeNormals = true;
            BuildBottomFace();
            BuildTopFace();
            BuildLeftFace();
            BuildRightFace();
            BuildBackFace();
            BuildFrontFace();

            return base.BuildMeshData();
        }

        protected override Vector3[] BuildVertices()
        {
            return _vertices.ToArray();
        }

        protected override int[] BuildIndices()
        {
            return _indices.ToArray();
        }

        protected override Vector3[] BuildNormals()
        {
            return _normals.ToArray();
        }


        // Bottom face
        private void BuildBottomFace()
        {
            // Vertices
            var vertices = base.BuildVertices();
            _vertices.AddRange(vertices);


            // Inidices
            _indices.AddRange(base.BuildIndices());
            for (var i = 0; i < _indices.Count; i += 3)
                (_indices[i], _indices[i + 2]) = (_indices[i + 2], _indices[i]);


            // Normals
            foreach (var (i, j) in VertexIndices())
            {
                var (u, v) = UVFromIndices(i, j);
                var topNormal = Surface.GetNormalAt(u, v);
                _normals.Add(new Vector3(topNormal.x, -topNormal.y, topNormal.z));
            }
        }


        // Top face
        private void BuildTopFace()
        {
            // Vertices
            var totVertices = _vertices.Count;
            for (var i = 0; i < totVertices; i++)
                _vertices.Add(_vertices[i] + Vector3.up * Height);


            // Indices
            var totIndices = _indices.Count;
            for (int i = 0; i < totIndices; i++)
                _indices.Add(_indices[i] + totVertices);
            for (var i = totIndices; i < _indices.Count; i += 3)
                (_indices[i], _indices[i + 2]) = (_indices[i + 2], _indices[i]);


            // Normals 
            foreach (var (i, j) in VertexIndices())
            {
                var (u, v) = UVFromIndices(i, j);
                _normals.Add(Surface.GetNormalAt(u, v));
            }
        }


        // Left Face
        private void AddTriangle(int index1, int index2, int index3, bool clockWiseNormals = true)
        {
            // Vertices
            var vertices = new[] { index1, index2, index3 }.Select(i => _vertices[i]).ToList();
            _vertices.AddRange(vertices);

            // Indices
            _indices.AddRange(new[] { _vertices.Count - 1, _vertices.Count - 2, _vertices.Count - 3 });

            // Normals
            var du = vertices[2] - vertices[1];
            var dv = vertices[1] - vertices[0];
            var normal = Vector3.Cross(dv, du).normalized;
            if (!clockWiseNormals)
                normal *= -1;
            _normals.Add(normal);
            _normals.Add(normal);
            _normals.Add(normal);
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
                    AddTriangle(b1, t1, b2, clockWiseNormals: false);
                    AddTriangle(b2, t1, t2, clockWiseNormals: false);
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
                    AddTriangle(b2, t1, b1, clockWiseNormals: false);
                    AddTriangle(t2, t1, b2, clockWiseNormals: false);
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
                    AddTriangle(b2, t1, b1, false);
                    AddTriangle(t2, t1, b2, false);
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

            // Bottom map
            for (var i = 0; i < TotVertices; i++)
                uvs.Add(new Vector2(0, 0));


            // Top map
            foreach (var uv in TextureUVMap())
                uvs.Add(uv);


            // Borders map
            for (var i = 2 * TotVertices; i < _vertices.Count; i++)
                uvs.Add(new Vector2(0, 0));
            return uvs.ToArray();
        }


        private IEnumerable<Vector2> TextureUVMap()
        {
            Vector3 NewPoint(float u)
            {
                Surface.TryGetPointAt(u, 0.5f * (Surface.VMin + Surface.UMax), out var point);
                return point;
            }

            var currentPoint = NewPoint(Surface.UMin);
            for (var i = 0; i < UVertexCount; i++)
            {
                var u = UFromIndex(i);
                var newPoint = NewPoint(u);
                _totDistanceTop += Vector3.Distance(newPoint, currentPoint);

                var textureU = _totDistanceTop * TextureScaleFactor;
                for (var j = 0; j < VVertexCount; j++)
                {
                    var v = VFromIndex(j);
                    var textureV = (v - Surface.VMin) / Surface.VLength;
                    yield return new Vector2(textureU, textureV);
                }

                currentPoint = newPoint;
            }
        }
    }
}
