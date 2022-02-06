using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.FinishLine
{
    public class FinishLineManager : MonoBehaviour
    {
        // Events
        public static event Action FinishLineReached;


        
        // Events handlers
        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag(UnityTag.Player))
                return;

            FinishLineReached?.Invoke();
        }



        // Cleaning events handlers on destroy
        private void OnDestroy()
        {
            FinishLineReached = null;
        }
    }
}
