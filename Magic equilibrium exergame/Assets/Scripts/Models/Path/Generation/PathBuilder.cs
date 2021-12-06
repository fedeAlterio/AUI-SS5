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
    public class PathBuilder<T> : ILineBuilder<T>, IBuilderStep1<T>, IBuilderStep2<T>, IBuilderStep3<T> where T : class, ILineBlock
    {
        // Private fields
        private readonly List<T> _surfaces = new List<T>();
        private readonly Func<CurveSurface, T> _mapper;
        private Vector3 _startPosition;
        private Vector3 _starttDirection;

        private bool _currentLineHasCurve;




        // initialization
        protected PathBuilder(Func<CurveSurface, T> mapper)
        {
            _mapper = mapper;
        }


        public static IBuilderStep1<T> New(Func<CurveSurface, T> mapper)
        {
            return new PathBuilder<T>(mapper);
        }


        public IBuilderStep2<T> WithDimensions(float curveWidth, float pathThickenss, float pathHeight)
        {
            PathHeight = pathHeight;
            CurveSize = curveWidth;
            Thickness = pathThickenss;

            return this;
        }


        public IBuilderStep3<T> WithTextureScaleFactor(float textureScaleFactor)
        {
            TextureScaleFactor = textureScaleFactor;
            return this;
        }


        public ILineBuilder<T> Start(Vector3 startPosition, Vector3 startDirection)
        {
            _startPosition = startPosition;
            _starttDirection = startDirection.normalized;
            return this;
        }



        // Properties
        private Vector3 CurrentPosition => _surfaces.Any() ? _surfaces[_surfaces.Count - 1].ExitPosition : _startPosition;
        private Vector3 CurrentDirection => _surfaces.Any() ? _surfaces[_surfaces.Count - 1].ExitDirection : _starttDirection;
        public int SegmentVertexCount { get; set; } = 3;
        public int CurveVertexCount { get; set; } = 40;
        public float CurveSize { get; set; } = 3;
        public float Thickness { get; set; } = 4;
        public float TextureScaleFactor { get; set; } = 0.25f;
        public float PathHeight { get; set; } = 0.1f;
        private T CurrentCurve => _currentLineHasCurve ? _surfaces[_surfaces.Count - 2] : null;
        private T CurrentSegment
        {
            get => _surfaces[_surfaces.Count - 1];
            set => _surfaces[_surfaces.Count - 1] = value;
        }


        // Line Builder
        public ILineBuilder<T> With(Func<T, T> map)
        {            
            if (CurrentSegment != null)
                CurrentSegment = map(CurrentSegment);

            //if(_currentCurve != null)
            //    map(_currentCurve);

            return this;
        }


        public ILineBuilder<T> Go(Vector3 nextPointRelativePosition)
        {
            return MoveOf(nextPointRelativePosition, NormalSegment, NormalCurve);
        }



        public ILineBuilder<T> GoWithHole(Vector3 nextPointDeltaPos, float startPercentage, float width, bool curveWithHole)
        {
            return MoveOf(nextPointDeltaPos, 
                segment => SegmentWithHole(segment, startPercentage, width),
                curve => curveWithHole ? CurveWithHole(curve, startPercentage, width) : NormalCurve(curve));
        }


        public IReadOnlyList<T> Build()
        {
            return _surfaces;
        }





        // Path creation strategies
        public T AddSurface(CurveSurface curve)
        {
            var ret =_mapper.Invoke(curve);
            _surfaces.Add(ret);
            curve.TextureScaleFactor = TextureScaleFactor;
            return ret;
        }



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
        private ILineBuilder<T> MoveOf(Vector3 nextPointRelativePosition, 
            Func<ParametricCurve, CurveSurface> segmentToSurface, Func<QuadraticBezier, CurveSurface> bezierToSurface)
        {
            var deltaPos = ToPathTangentCoordinates(CurrentDirection, nextPointRelativePosition);
            _currentLineHasCurve = false;

            // If it does not change position do not create a cruve
            if(Vector3.Angle(deltaPos, CurrentDirection) > Mathf.Epsilon)
            {
                // Creating curve

                var bezierMiddle = CurrentPosition + CurrentDirection * CurveSize;
                var bezierEnd = bezierMiddle + deltaPos.normalized * CurveSize;
                var bezier = new QuadraticBezier(CurrentPosition, bezierMiddle, bezierEnd);
                var bezierSurface = bezierToSurface?.Invoke(bezier);
                AddSurface(bezierSurface);
                _currentLineHasCurve = true;
            }


            // Segment Creation
            var nextPoint = CurrentPosition + deltaPos;
            var forwardLine = Curves.Line(CurrentPosition, nextPoint);
            var segmentSurface = segmentToSurface?.Invoke(forwardLine);
            AddSurface(segmentSurface);
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
    public interface IBuilderStep1<T> where T : ILineBlock
    {
        public IBuilderStep2<T> WithDimensions(float curveWidth, float pathThickenss, float pathHeight);
    }
   

    public interface IBuilderStep2<T> where T : ILineBlock
    {
        IBuilderStep3<T> WithTextureScaleFactor(float textureScale);
    }

    public interface IBuilderStep3<T> where T : ILineBlock
    {
        ILineBuilder<T> Start(Vector3 startPosition, Vector3 startDirection);
    }

}
