using Assets.Scripts.Models.Path.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.GameDebug
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(MeshFilter))]
    public class MeshTest : MonoBehaviour
    {
        private void Start()
        {
               
        }


        private void Update()
        {
            var meshFilter = GetComponent<MeshFilter>();
            meshFilter.sharedMesh = new CurveMeshGenerator().FromCurve();
        }
    }
}
