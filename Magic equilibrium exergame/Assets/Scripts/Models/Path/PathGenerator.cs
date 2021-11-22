using Assets.Scripts.Models.Path.Blocks;
using Assets.Scripts.Models.Path.Generation;
using Assets.Scripts.Models.Path.Generation.Line;
using Assets.Scripts.Models.Path.Generation.Surface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Models.Path
{
    public class PathGenerator : MonoBehaviour
    {
        // Editor fields
        [SerializeField] private PathManager _pathManager;
        [SerializeField] private List<BaseBlock> _blocksPrefabs = new List<BaseBlock>();
        [SerializeField] private CurveBlock _curveBlock;



        // Private fields
        private readonly List<CurveBlock> _blocks = new List<CurveBlock>();



        // Initialization
        private void Start()
        {
        }



        // Events
        private void Update()
        {
            if(_oldPathThickness != PathThickness || _oldCurveSize != CurveSize)
            {
                _oldPathThickness = PathThickness;
                _oldCurveSize = CurveSize;
                GenerateLine();
            }
        }


        // Properties
        private float _oldPathThickness;
        [field:SerializeField] public float PathThickness { get; set; } = 4;


        private float _oldCurveSize;
        [field: SerializeField] public float CurveSize { get; set; } = 3;


        // Public
        public void GenerateLine()
        {
            ClearBlocks();
            var lines = LineBuilder.NewLine(Vector3.zero, Vector3.forward, CurveSize)
                .MoveOf(Vector3.forward * 10)
                .MoveOf(new Vector3(0, 1, 3).normalized * 5)
                .MoveOf(new Vector3(0, -1, 3).normalized * 5)
                .MoveOf(new Vector3(0, -1, 3).normalized * 5)
                .MoveOf(new Vector3(0, 1, 3).normalized * 5)
                .MoveOf(new Vector3(1, 0, 1) * 5)
                .Build();

            var surfaces = lines.Select(line => DiscreteSurfaces.FromDiscreteCurve(line, PathThickness));
            foreach (var surface in surfaces)
            {
                var block = BlockFromPrefab(_curveBlock);
                _blocks.Add(block);
                block.Initialize(surface);
            }
        }

        private void ClearBlocks()
        {
            foreach(var block in _blocks.ToList())
            {
                Destroy(block.gameObject);
                _blocks.Remove(block);
            }
        }


        // utils
        private T BlockFromPrefab<T>(T pathBlock) where T : BaseBlock
        {
            if (pathBlock == null)
                return default;
            var ret = Instantiate(pathBlock);
            ret.transform.parent = transform;
            //ret.name = pathBlock.BlockName;
            return ret;
        }
    }
}
