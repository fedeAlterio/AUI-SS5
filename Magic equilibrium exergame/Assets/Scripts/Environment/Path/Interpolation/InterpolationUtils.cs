using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Models.Path.Interpolation
{
    public static partial class InterpolationUtils
    {
        /// <summary>
        /// Interpolate a list of points to a list of ordered Bezier curve segments
        /// </summary>
        /// <param name="points"></param>
        /// <param name="isClosedCurve">True if is a closed curve</param>
        /// <param name="smoothValue">Optional value for making the curve smoother or not</param>
        /// <returns></returns>
        public static IList<BezierCurveSegment> PointsToBezierCurves(IList<Vector2> points, bool isClosedCurve, float smoothValue = 0.8f)
        {
            if (points.Count < 3)
                return new List<BezierCurveSegment>();
            var toRet = new List<BezierCurveSegment>();

            //if is close curve then add the first point at the end
            if (isClosedCurve)
                points.Add(points.First());

            for (int i = 0; i < points.Count - 1; i++)   //iterate for points but the last one
            {
                // Assume we need to calculate the control
                // points between (x1,y1) and (x2,y2).
                // Then x0,y0 - the previous vertex,
                //      x3,y3 - the next one.
                float x1 = points[i].x;
                float y1 = points[i].y;
                
                float x2 = points[i + 1].x;
                float y2 = points[i + 1].y;
                
                float x0;
                float y0;

                if (i == 0) //if is first point
                {
                    if (isClosedCurve)
                    {
                        var previousPoint = points[points.Count - 2];    //last Point, but one (due inserted the first at the end)
                        x0 = previousPoint.x;
                        y0 = previousPoint.y;
                    }
                    else    //Get some previouse point
                    {
                        var previousPoint = points[i];  //if is the first point the previous one will be it self
                        x0 = previousPoint.x;
                        y0 = previousPoint.y;
                    }
                }
                else
                {
                    x0 = points[i - 1].x;   //Previous Point
                    y0 = points[i - 1].y;
                }

                float x3, y3;

                if (i == points.Count - 2)    //if is the last point
                {
                    if (isClosedCurve)
                    {
                        var nextPoint = points[1];  //second Point(due inserted the first at the end)
                        x3 = nextPoint.x;
                        y3 = nextPoint.y;
                    }
                    else    //Get some next point
                    {
                        var nextPoint = points[i + 1];  //if is the last point the next point will be the last one
                        x3 = nextPoint.x;
                        y3 = nextPoint.y;
                    }
                }
                else
                {
                    x3 = points[i + 2].x;   //Next Point
                    y3 = points[i + 2].y;
                }

                float xc1 = (x0 + x1) / 2;
                float yc1 = (y0 + y1) / 2;
                float xc2 = (x1 + x2) / 2;
                float yc2 = (y1 + y2) / 2;
                float xc3 = (x2 + x3) / 2;
                float yc3 = (y2 + y3) / 2;

                float len1 = Helpers.EuclideanDistance(x0, y0, x1, y1);
                float len2 = Helpers.EuclideanDistance(x1, y1, x2, y2);
                float len3 = Helpers.EuclideanDistance(x2, y2, x3, y3);
                
                float k1 = len1 / (len1 + len2);
                float k2 = len2 / (len2 + len3);
                
                float xm1 = xc1 + (xc2 - xc1) * k1;
                float ym1 = yc1 + (yc2 - yc1) * k1;
                
                float xm2 = xc2 + (xc3 - xc2) * k2;
                float ym2 = yc2 + (yc3 - yc2) * k2;

                // Resulting control points. Here smooth_value is mentioned
                // above coefficient K whose value should be in range [0...1].
                float ctrl1_x = xm1 + (xc2 - xm1) * smoothValue + x1 - xm1;
                float ctrl1_y = ym1 + (yc2 - ym1) * smoothValue + y1 - ym1;
               
                float ctrl2_x = xm2 + (xc2 - xm2) * smoothValue + x2 - xm2;
                float ctrl2_y = ym2 + (yc2 - ym2) * smoothValue + y2 - ym2;
                toRet.Add(new BezierCurveSegment
                {
                    StartPoint = new Vector2(x1, y1),
                    EndPoint = new Vector2(x2, y2),
                    FirstControlPoint = i == 0 && !isClosedCurve ? new Vector2(x1, y1) : new Vector2(ctrl1_x, ctrl1_y),
                    SecondControlPoint = i == points.Count - 2 && !isClosedCurve ? new Vector2(x2, y2) : new Vector2(ctrl2_x, ctrl2_y)
                });
            }

            return toRet;
        }
    }
}
