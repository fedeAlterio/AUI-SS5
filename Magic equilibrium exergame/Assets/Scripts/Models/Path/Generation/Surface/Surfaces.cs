using Assets.Scripts.Models.Path.Generation.Line;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Models.Path.Generation.Surface
{
    public static class Surfaces
    {
        public static ParametricSurface Plane()
        {
            SurfaceEquation equation = (u, v) => new Vector3(-u, 0, v);
            return new ParametricSurface(equation, -1, 1, -1, 1);
        }

        public static ParametricSurface Sphere()
        {
            SurfaceEquation equation = (u, v) => new Vector3(Mathf.Cos(u) * Mathf.Sin(v), Mathf.Sin(u) * Mathf.Sin(v), Mathf.Cos(v));
            return new ParametricSurface(equation, 0, 2 * Mathf.PI, 0, Mathf.PI);
        }

        public static ParametricSurface Circle()
        {
            SurfaceEquation equation = (u, v) => new Vector3(v * Mathf.Cos(u), v * Mathf.Sin(u), 0);
            return new ParametricSurface(equation, 0, Mathf.PI * 2, 0, 1);
        }

        public static ParametricSurface FromCurve(ParametricCurve curve, float thickness)
        {
            Vector3 Equation(float s, float n)
            {
                var center = curve.PointAt(s);
                var normal = curve.NormalAt(s);
                return center + normal * n;
            }

            return new ParametricSurface(Equation, curve.MinT, curve.MaxT, -thickness / 2, thickness / 2);
        }

        public static PiercedSurface PlaneWithHole()
        {
            var plane = Plane();
            bool TryGetPointAt(float u, float v, out Vector3 point)
            {
                point = plane.PointAt(u, v);
                var radius = 0.5f;
                if (Mathf.Abs(u) < radius && Mathf.Abs(v) < radius)
                    return false;
                return true;
            }
            return new PiercedSurface(TryGetPointAt, plane.UMin, plane.UMax, plane.VMin, plane.VMax);
        }
    }
}
