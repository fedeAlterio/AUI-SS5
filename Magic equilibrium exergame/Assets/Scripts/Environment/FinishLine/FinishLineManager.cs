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
        private event Action FinishLineReached;


        
        // Events handlers
        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag(UnityTag.Player))
                return;

            Debug.Log("Finish");
            FinishLineReached?.Invoke();
        }
    }
}
