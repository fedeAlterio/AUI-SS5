using Assets.Scripts.Models.Path.Generation.Line;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models.Path.Generation.Surface
{
    public static class DiscreteSurfaces
    {
        public static CurveSurface FromDiscreteCurve(DiscreteCurve curve, float thickness)
        {
            return new CurveSurface(curve, thickness);
        }
    }
}
