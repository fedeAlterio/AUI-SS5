using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Abstractions
{
    public interface IWobbleboardService
    {
        public float XAngle { get; }
        public float ZAngle { get; }
    }
}
