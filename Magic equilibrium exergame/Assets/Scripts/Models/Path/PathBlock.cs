using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Models.Path
{
    public class PathBlock : MonoBehaviour
    {
        // Editor fields
        [SerializeField] private Transform _entryTransform;
        [SerializeField] private Transform _exitTransform;
        [SerializeField] private Vector3 _entryDirection;
        [SerializeField] private Vector3 _exitDirection;



        // Properties
        public Vector3 EntryPosition => _entryTransform.position;
        public Vector3 ExitPosition => _exitTransform.position;
        public Vector3 Position => transform.position;
        public Vector3 EntryDirection => transform.TransformDirection(_entryDirection);
        public Vector3 ExitDirection => transform.TransformDirection(_exitDirection);
        [field:SerializeField] public string BlockName { get; private set; }



        // Debug
        private void OnDrawGizmosSelected()
        {            
            Debug.DrawLine(EntryPosition, EntryPosition + EntryDirection, Color.blue, 0, false);
            Debug.DrawLine(ExitPosition, ExitPosition + ExitDirection, Color.red, 0, false);
        }
    }
}