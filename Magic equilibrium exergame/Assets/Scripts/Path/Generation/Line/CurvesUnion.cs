using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Models.Path.Generation.Line
{
    public class CurvesUnion : ParametricCurve
    {
        // Private fields
        private List<ParametricCurve> _curves = new List<ParametricCurve>();



        // Initialization
        public CurvesUnion(IEnumerable<ParametricCurve> curves)
        {
            if (!curves.Any())
                throw new InvalidOperationException();

            var startTime = curves.First().MinT;
            var currentTime = startTime;

            foreach (var curve in curves)
            {
                var realCurve = curve;
                var currentStartTime = currentTime;
                CurveEquation equation = t => curve.PointAt(t - (currentStartTime - realCurve.MinT));
                currentTime += curve.DeltaTime;
                _curves.Add(new ParametricCurve(equation, currentStartTime, currentStartTime + realCurve.DeltaTime));
            }

            _equation = t => CurveByTime(t).PointAt(t);
            MinT = startTime;
            MaxT = currentTime;
        }


        // Core
        private ParametricCurve CurveByTime(float t)
        {
            if (t < MinT)
                return _curves[0];

            if (t > MaxT)
                return _curves[_curves.Count - 1];

            var (start, end) = (0, _curves.Count - 1);
            while (start < end)
            {
                var centerIndex = (start + end) / 2;
                var curve = _curves[centerIndex];
                (start, end) = t >= curve.MaxT
                    ? (centerIndex + 1, end)
                    : (start, centerIndex);
            }

            return _curves[start];
        }

    }
}
