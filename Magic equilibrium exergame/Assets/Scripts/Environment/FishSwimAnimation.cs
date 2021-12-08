using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Environment
{
    public class FishSwimAnimation : MonoBehaviour
    {
        // Editor fields
        [SerializeField] private MeshRenderer _meshFilter;
        [SerializeField] private float _swimAnimationSpeed;



        // Private fields
        private bool _goingForward = true;



        // Initialization
        private void Start()
        {
            WaveTime = 0;
        }



        // Properties
        private float WaveTime
        {
            get => _meshFilter.material.GetFloat("_" + nameof(WaveTime));
            set => _meshFilter.material.SetFloat("_" + nameof(WaveTime), value);
        }



        // Events
        private void Update()
        {
            if(WaveTime == 1)
                _goingForward = false;
            else if(WaveTime == -1)
                _goingForward = true;

            var forwardFactor = _goingForward ? 1 : -1;
            var newSwimPercentage = Mathf.Clamp(WaveTime + _swimAnimationSpeed * forwardFactor * Time.smoothDeltaTime, -1, 1);
            WaveTime = newSwimPercentage;
        }
    }
}
