using Assets.Scripts.Models.Path.Generation.Line;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models.Path.Generation.Surface
{
    public class CurveSurface : DiscreteSurface
    {
        public CurveSurface(DiscreteCurve curve, float thickness) : base(Surfaces.FromCurve(curve.Curve, thickness))
        {
            DiscreteCurve = curve;
            Thickness = thickness;
            UVertexCount = curve.VertexCount;
            VVertexCount = 3;
        }

        
        
        // Properties
        public DiscreteCurve DiscreteCurve { get; }
        public float Thickness { get; }
    }
}
