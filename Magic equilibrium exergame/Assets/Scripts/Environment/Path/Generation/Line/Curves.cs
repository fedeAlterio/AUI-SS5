using Assets.Scripts.Models.Path.Interpolation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Models.Path.Generation.Line
{
    public static class Curves
    {
        public static ParametricCurve Line(Vector3 entry, Vector3 exit)
        {
            CurveEquation equation = t => entry + t * (exit - entry);
            return new ParametricCurve(equation, 0, 1);
        }


        public static ParametricCurve Circle(Vector3 center, float radius, float speed = 1)
        {
            var w = speed / radius;
            CurveEquation equation = t => radius * new Vector3(Mathf.Cos(w*t), 0, Mathf.Sin(w*t)) + center;
            return new ParametricCurve(equation, 0, 2 * Mathf.PI / w);
        }


        public static IEnumerable<ParametricCurve> GetBorderFromTexture(Texture2D texture)
        {            
            var points = TextureUtils.GetBorderPoints(texture).ToList();
            
            var borderPoints = points.ToDictionary(x => x, x => true);
            var line = RemoveConnectedLine(borderPoints).Select(p => new Vector2(p.x, p.y) / 10).ToList();
            line = SmoothPoints(line, 14);
            while (line.Any())
            {
                var bezierSegments = InterpolationUtils.PointsToBezierCurves(line, false)
                    .Select(x => new CubicBezier(FromVector2(x.StartPoint), FromVector2(x.EndPoint), FromVector2(x.FirstControlPoint), FromVector2(x.SecondControlPoint)));
                foreach (var curve in bezierSegments)
                    yield return curve;
                line = RemoveConnectedLine(borderPoints).Select(p => new Vector2(p.x, p.y) / 10).ToList();
                line = SmoothPoints(line, 14);
            }
        }




        // Utils
        private static Vector3 FromVector2(Vector2 v)
        {
            return new Vector3(v.x, 0, v.y);
        }


        private static IEnumerable<(int x, int y)> RemoveConnectedLine(Dictionary<(int x, int y), bool> points)
        {
            bool TryGetClosest(int x, int y, out (int x, int y) closePoint)
            {
                var toTest = new[]{ (x+1, y), (x-1, y), (x, y+1), (x, y-1), (x+1, y+1), (x+1, y-1), (x-1, y+1), (x-1, y-1) };
                foreach (var point in toTest)
                    if (points.TryGetValue(point, out var _))
                    {
                        closePoint = point;
                        return true;
                    }
                closePoint = default;
                return false;
            }

            if (!points.Any())
                yield break;

            var current = points.Keys.FirstOrDefault();
            do
            {
                points.Remove(current);
                yield return current;
            }
            while (TryGetClosest(current.x, current.y, out current));
        }


        private static List<Vector2> SmoothPoints(List<Vector2> points, int smoothFactor)
        {
            var ret = new List<Vector2>();
            for (var i = 0; i < points.Count; i += smoothFactor)
                ret.Add(points[i]);
            if(ret.Any())
                ret.Add(points[points.Count - 1]);
            return ret;
        }
    }
}
