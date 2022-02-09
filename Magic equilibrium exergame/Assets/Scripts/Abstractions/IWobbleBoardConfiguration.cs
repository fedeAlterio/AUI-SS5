using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Abstractions
{
    public interface IWobbleBoardConfiguration 
    {
        public float HorizontalRotationAngle { get; }
        public float MaxForwardlAngle { get; }
        public float MaxBackwardlAngle { get; }
        public float MaxRightAngle { get; }
        public float MaxLeftAngle { get; }
        public int Sensibility { get; }

    }
}
