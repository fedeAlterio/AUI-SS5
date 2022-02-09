using Assets.Scripts.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.PlayerMovement
{
    public class WASDMovementAxis : IMovementAxis
    {
        public float HorizontalAxis => Input.GetAxisRaw("Horizontal");
        public float VerticalAxis => Input.GetAxisRaw("Vertical");
    }
}
