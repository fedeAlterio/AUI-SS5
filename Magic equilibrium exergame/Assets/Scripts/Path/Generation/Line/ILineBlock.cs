using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Models.Path.Generation.Line
{
    public interface ILineBlock
    {
        Vector3 EntryPosition { get; }
        Vector3 ExitPosition { get; }
        Vector3 Position { get; }
        Vector3 EntryDirection { get; }
        Vector3 ExitDirection { get; }
    }
}
