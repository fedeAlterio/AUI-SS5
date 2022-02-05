using Assets.Scripts.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.WobbleBoardCalibration
{
    public class WobbleBoardConfiguration : MonoBehaviour, IWobbleBoardConfiguration
    {
        [field: SerializeField] public float HorizontalRotationAngle { get; set; } 
        [field: SerializeField] public float MaxForwardlAngle { get; set; } = Mathf.PI / 2;
        [field: SerializeField] public float MaxBackwardlAngle { get; set; } = -Mathf.PI / 2;
        [field: SerializeField] public float MaxHorizontalAngle { get; set; } = Mathf.PI / 2;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
