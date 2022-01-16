using Assets.Scripts.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Mock
{
    public class MockWobbleboardService : IWobbleboardService
    {
        public float XAngle => 0;
        public float ZAngle => 0;
    }
}
