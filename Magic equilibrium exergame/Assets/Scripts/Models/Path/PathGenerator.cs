using Assets.Scripts.Models.Path.Blocks;
using Assets.Scripts.Models.Path.Generation;
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
        [SerializeField] private Sprite _sprite;



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
        [field: SerializeField] public float CurveSize { get; set; } = 4;



        // Public
        public void GenerateLine()
        {            
            _pathManager.Clear();
            var surfaces = PathBuilder.NewLine(Vector3.zero, Vector3.right, CurveSize, PathThickness)
                .Go(Vector3.forward * 5)
                .GoWithHole(Vector3.forward * 10, 0, 0.3f)      
                .Go(Vector3.forward * 5)
                .GoWithHole(new Vector3(-1,0,2).normalized * 5, 0.75f, 0.25f, curveWithHole: true)
                .GoWithHole(new Vector3(0, 1, 3).normalized * 10, 0.75f, 0.25f, curveWithHole: true)
                .Build();

            foreach (var surface in surfaces)
            {
                var curveBlock = BlockFromPrefab(_curveBlock);
                curveBlock.Initialize(surface);
                _pathManager.Add(curveBlock);
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
