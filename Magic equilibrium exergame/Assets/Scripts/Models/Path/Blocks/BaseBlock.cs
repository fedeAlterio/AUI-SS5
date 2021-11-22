using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Models.Path.Blocks
{
    public abstract class BaseBlock : MonoBehaviour
    {
        public virtual Vector3 EntryPosition { get; }
        public virtual Vector3 ExitPosition { get; }
        public virtual Vector3 Position { get; }
        public virtual Vector3 EntryDirection { get; }
        public virtual Vector3 ExitDirection { get; }
        public virtual string BlockName { get; protected set; }
    }
}
