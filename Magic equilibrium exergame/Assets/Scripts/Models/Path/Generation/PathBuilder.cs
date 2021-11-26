using Assets.Scripts.Models.Path.Generation.Line;
using Assets.Scripts.Models.Path.Generation.Surface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Models.Path.Generation
{
    public class PathBuilder : ILineBuilder, IBuilderStep1, IBuilderStep2
    {
        // Private fields
        private readonly List<CurveSurface> _surfaces = new List<CurveSurface>();
        private Vector3 _currentPosition;
        private Vector3 _currentDirection;
        private Material _material;



        // initialization
        protected PathBuilder(float curveWidth, float pathThickenss, float pathHeight)
        {
            PathHeight = pathHeight;
            CurveSize = curveWidth;
            Thickness = pathThickenss;
        }


        public static IBuilderStep1 NewLine(float curveWidth, float pathThickenss, float pathHeight)
        {
            var ret = new PathBuilder(curveWidth, pathThickenss, pathHeight);
            return ret;
        }

        public IBuilderStep2 WithTextureScaleFactor(float textureScaleFactor)
        {
            TextureScaleFactor = textureScaleFactor;
            return this;
        }



        public ILineBuilder Start(Vector3 startPosition, Vector3 startDirection)
        {
            _currentPosition = startPosition;
            _currentDirection = startDirection.normalized;
            return this;
        }




        // Properties
        public int SegmentVertexCount { get; set; } = 3;
        public int CurveVertexCount { get; set; } = 40;
        public float CurveSize { get; set; } = 3;
        public float Thickness { get; set; } = 4;
        public float TextureScaleFactor { get; set; } = 0.25f;
        public float PathHeight { get; set; } = 0.1f;



        // Line Builder
        public ILineBuilder Go(Vector3 nextPointRelativePosition)
        {
            return MoveOf(nextPointRelativePosition, NormalSegment, NormalCurve);
        }



        public ILineBuilder GoWithHole(Vector3 nextPointDeltaPos, float startPercentage, float width, bool curveWithHole)
        {
            return MoveOf(nextPointDeltaPos, 
                segment => SegmentWithHole(segment, startPercentage, width),
                curve => curveWithHole ? CurveWithHole(curve, startPercentage, width) : NormalCurve(curve));
        }


        public IReadOnlyList<CurveSurface> Build()
        {
            return _surfaces;
        }



        // Path creation strategies


        // Segment
        private CurveSurface NormalSegment(ParametricCurve segment)
        {
            var discreteCurve = new DiscreteCurve(segment) { VertexCount = SegmentVertexCount };
            return new CurveSurface(discreteCurve, Thickness, PathHeight);
        }

        private CurveSurface SegmentWithHole(ParametricCurve segment, float nStart, float width)
        {
            var ret = PathWithHole(segment, nStart, width);
            ret.UVertexCount = SegmentVertexCount;
            ret.VVertexCount = CurveVertexCount;
            return ret;
        }



        // Curve
        private CurveSurface NormalCurve(ParametricCurve curve)
        {
            var discreteBezier = new DiscreteCurve(curve) { VertexCount = CurveVertexCount };
            var bezierSurface = new CurveSurface(discreteBezier, Thickness, PathHeight);
            return bezierSurface;
        }

        private CurveSurface CurveWithHole(ParametricCurve curve, float nStart, float width)
        {
            var ret = PathWithHole(curve, nStart, width);
            ret.UVertexCount = CurveVertexCount;
            ret.VVertexCount = CurveVertexCount;
            return ret;
        }



        // Generic
        private CurveSurface PathWithHole(ParametricCurve curve, float nStart, float width)
        {
            var surface = Surfaces.FromCurve(curve, Thickness);
            var deltaN = surface.VMax - surface.VMin;
            var startN = surface.VMin + nStart * deltaN;
            var endN = startN + width * deltaN;

            bool TryGetPointAt(float s, float n, out Vector3 point)
            {
                point = surface.PointAt(s, n);
                return n < startN || n > endN;
            }
            var piercedSurface = PiercedSurface.FromParametricSurface(surface, TryGetPointAt);
            var curveSurface = new CurveSurface(piercedSurface, curve, Thickness, PathHeight);
            return curveSurface;
        }

        private CurveSurface TextureHole(ParametricCurve curve, Texture2D texture)
        {
            var surface = Surfaces.FromCurve(curve, Thickness);
            var deltaU = surface.UMax - surface.UMin;
            var deltaV = surface.VMax - surface.VMin;
            (int x, int y) UVToTextureCoords(float u, float v)
            {
                var xPercentage = (u - surface.UMin) / deltaU;
                var yPercentage = (v - surface.VMin) / deltaV;
                var x = (int)(xPercentage * texture.width);
                var y = (int)(yPercentage * texture.height);
                x = Mathf.Clamp(x, 0, texture.width);
                y = Mathf.Clamp(y, 0, texture.height);
                return (x, y);
            }
            bool TryGetPointAt(float s, float n, out Vector3 point)
            {
                var (x, y) = UVToTextureCoords(s, n);
                var color = texture.GetPixel(x, y);
                var epsilon = 0.1f;
                point = surface.PointAt(s, n);
                if (color.r < epsilon && color.g < epsilon && color.b < epsilon)
                    return false;
                return true;
            }
            var piercedSurface = PiercedSurface.FromParametricSurface(surface, TryGetPointAt);
            var curveSurface = new CurveSurface(piercedSurface, curve, Thickness, PathHeight);
            curveSurface.UVertexCount = 100;
            curveSurface.VVertexCount = 100;
            return curveSurface;


        }




        // Utils        
        private void AddSurface(CurveSurface curveSurface)
        {
            curveSurface.TextureScaleFactor = TextureScaleFactor;
            _surfaces.Add(curveSurface);
        }

        private ILineBuilder MoveOf(Vector3 nextPointRelativePosition, 
            Func<ParametricCurve, CurveSurface> segmentToSurface, Func<QuadraticBezier, CurveSurface> bezierToSurface)
        {
            var deltaPos = ToPathTangentCoordinates(_currentDirection, nextPointRelativePosition);
            
            // If it does not change position do not create a cruve
            if(Vector3.Angle(deltaPos, _currentDirection) > Mathf.Epsilon)
            {
                // Creating curve

                var bezierMiddle = _currentPosition + _currentDirection * CurveSize;
                var bezierEnd = bezierMiddle + deltaPos.normalized * CurveSize;
                var bezier = new QuadraticBezier(_currentPosition, bezierMiddle, bezierEnd);
                var bezierSurface = bezierToSurface?.Invoke(bezier);
                _currentPosition = bezierEnd;
                AddSurface(bezierSurface);
            }


            // Segment Creation
            var nextPoint = _currentPosition + deltaPos;
            var forwardLine = Curves.Line(_currentPosition, nextPoint);
            var segmentSurface = segmentToSurface?.Invoke(forwardLine);
            AddSurface(segmentSurface);


            // Next point setup
            _currentPosition = nextPoint;
            _currentDirection = forwardLine.TangentAt(forwardLine.MaxT).normalized;

            return this;
        }        


        private Vector3 ToPathTangentCoordinates(Vector3 pathTangentDirection, Vector3 direction)
        {
            var z = pathTangentDirection.normalized;
            var x = Vector3.Cross(Vector3.up, z);
            var y = Vector3.Cross(z, x);

            return direction.x * x + direction.y * y + direction.z * z;
        }


    }
    public interface IBuilderStep1
    {
        IBuilderStep2 WithTextureScaleFactor(float textureScale);
    }
   

    public interface IBuilderStep2
    {
        ILineBuilder Start(Vector3 startPosition, Vector3 startDirection);
    }

}
