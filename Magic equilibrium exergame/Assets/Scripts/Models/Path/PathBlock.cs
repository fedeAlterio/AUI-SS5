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


        // Properties
        public Vector3 EntryPosition => _entryTransform.position;
        public Vector3 ExitPosition => _exitTransform.position;
        public Vector3 Position => transform.position;      
        [field:SerializeField] public string BlockName { get; private set; }
    }
}