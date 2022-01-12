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
    public interface ILineBuilder<T> where T : ILineBlock
    {
        ILineBuilder<T> With(Func<T, T> map);
        ILineBuilder<T> Go(Vector3 nextPointDeltaPos, bool isTangentSpace = true);
        ILineBuilder<T> GoWithHole(Vector3 nextPointDeltaPos, float start, float width, bool curveWithHole = true, bool isTangentSpace = true);
        ILineBuilder<T> GoWithThinPath(Vector3 nextPointDeltaPos, float width, bool thinCurve = true, bool isTangentSpace = true);
        IReadOnlyList<T> Build();
        Vector3 CurrentDirection { get; }
        Vector3 CurrentPosition { get; }        
    }
}
