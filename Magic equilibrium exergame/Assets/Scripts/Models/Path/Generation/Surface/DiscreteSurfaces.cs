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
        public static DiscreteSurface FromDiscreteCurve(DiscreteCurve curve, float thickness)
        {
            var surface = Surfaces.FromCurve(curve.Curve, thickness);
            var discreteSurface = new DiscreteSurface(surface)
            {
                UVertexCount = curve.VertexCount,
                VVertexCount = 3
            };
            return discreteSurface;
        }
    }
}
