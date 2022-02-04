using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.PlayerMovement
{
    public class WASDMovement : PlayerMovementAbstract
    {
        public override float HorizontalAxis => Input.GetAxis("Horizontal");
        public override float VerticalAxis => Input.GetAxis("Vertical");
    }
}
