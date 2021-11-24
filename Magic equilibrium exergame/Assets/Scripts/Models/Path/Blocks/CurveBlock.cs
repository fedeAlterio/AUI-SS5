using Assets.Scripts.Models.Path.Generation;
using Assets.Scripts.Models.Path.Generation.Line;
using Assets.Scripts.Models.Path.Generation.Surface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Models.Path.Blocks
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class CurveBlock : BaseBlock
    {
        // Private fields;
        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;



        // Initialization
        private void Awake()
        {
            _meshFilter = GetComponent<MeshFilter>();
            _meshRenderer = GetComponent<MeshRenderer>();
        }



        // Properties
        public override Vector3 EntryPosition => Curve.PointAt(Curve.MinT);
        public override Vector3 ExitPosition => Curve.PointAt(Curve.MaxT);
        public override Vector3 Position => transform.position;
        public override Vector3 EntryDirection => Curve.TangentAt(Curve.MinT);
        public override Vector3 ExitDirection => Curve.TangentAt(Curve.MaxT);
        public CurveSurface Surface { get; private set; }
        public ParametricCurve Curve => Surface.Curve;



        // Public Methods
        public void Initialize(CurveSurface surface, Material material)
        {
            Surface = surface;
            var mesh = surface.BuildMesh();
            _meshFilter.mesh = mesh;
            _meshRenderer.material = material;

            gameObject.AddComponent<MeshCollider>();
        }
    }
}
