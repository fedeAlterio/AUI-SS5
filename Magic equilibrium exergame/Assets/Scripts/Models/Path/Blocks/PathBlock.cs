using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Models.Path.Blocks
{
    public class PathBlock : BaseBlock
    {
        // Editor fields
        [SerializeField] private Transform _entryTransform;
        [SerializeField] private Transform _exitTransform;
        [SerializeField] private Vector3 _entryDirection;
        [SerializeField] private Vector3 _exitDirection;



        // Properties
        public override Vector3 EntryPosition => _entryTransform.position;
        public override Vector3 ExitPosition => _exitTransform.position;
        public override Vector3 Position => transform.position;
        public override Vector3 EntryDirection => transform.TransformDirection(_entryDirection);
        public override Vector3 ExitDirection => transform.TransformDirection(_exitDirection);
        public override string BlockName { get; protected set; }



        // Debug
        private void OnDrawGizmosSelected()
        {
            Debug.DrawLine(EntryPosition, EntryPosition + EntryDirection, Color.blue, 0, false);
            Debug.DrawLine(ExitPosition, ExitPosition + ExitDirection, Color.red, 0, false);
        }
    }
}