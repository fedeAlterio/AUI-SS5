using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models.Path.Generation.Line
{
    public class DiscreteCurve
    {
        // Initialization
        public DiscreteCurve(ParametricCurve curve)
        {
            Curve = curve;
        }



        // Properties
        public ParametricCurve Curve { get; }
        public int VertexCount { get; set; } = 20;
    }
}
