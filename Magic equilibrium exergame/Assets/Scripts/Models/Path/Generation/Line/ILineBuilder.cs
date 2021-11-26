using Assets.Scripts.Models.Path.Generation;
using Assets.Scripts.Models.Path.Generation.Surface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Models.Path.Generation.Line
{
    public interface ILineBuilder
    {
        ILineBuilder Go(Vector3 nextPointDeltaPos);
        ILineBuilder GoWithHole(Vector3 nextPointDeltaPos, float start, float width, bool curveWithHole = true);
        IReadOnlyList<CurveSurface> Build();
    }
}
