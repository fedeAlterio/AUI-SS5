﻿using Assets.Scripts.Models.Path.Generation;
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
        ILineBuilder MoveOf(Vector3 nextPointRelativePosition);
        IEnumerable<DiscreteCurve> Build();
    }
}
