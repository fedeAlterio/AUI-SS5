using Assets.Scripts.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.DepndencyInjection.Mocks
{
    public class MockWobbleBoardConfiguration : IWobbleBoardConfiguration
    {
        public float HorizontalRotationAngle => 0;
        public float MaxForwardlAngle => Mathf.PI / 2;
        public float MaxBackwardlAngle => -Mathf.PI / 2;
        public float MaxHorizontalAngle => Mathf.PI / 2;
    }
}
