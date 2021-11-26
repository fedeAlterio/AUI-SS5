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
        [SerializeField] private Material _pathMaterial;



        // Private fields
        private readonly List<CurveBlock> _blocks = new List<CurveBlock>();



        // Initialization
        private void Start()
        {
        }



        // Events
        private void Update()
        {
            if(_oldPathThickness != PathThickness || _oldCurveSize != CurveSize  || _oldTextureScale != TextureScale || _oldPathHeight != PathHeight)
            {
                _oldPathHeight = PathHeight;
                _oldPathThickness = PathThickness;
                _oldCurveSize = CurveSize;
                _oldTextureScale = TextureScale;
                GenerateLine();
            }
        }


        // Properties
        private float _oldPathThickness;
        [field:SerializeField] public float PathThickness { get; set; } = 4;


        private float _oldCurveSize;
        [field: SerializeField] public float CurveSize { get; set; } = 4;


        private float _oldTextureScale;
        [field: SerializeField] public float TextureScale { get; set; } = 0.25f;



        private float _oldPathHeight;
        [field: SerializeField] public float PathHeight { get; set; } = 0.1f;


        // Public
        public void GenerateLine()
        {            
            _pathManager.Clear();
            var surfaces = PathBuilder.NewLine(CurveSize, PathThickness, PathHeight)
                .WithTextureScaleFactor(TextureScale)
                .Start(Vector3.zero, Vector3.right)
                .Go(Vector3.forward * 10)
                .GoWithHole(new Vector3(0,1,3).normalized * 10, 0.3f, 0.3f, true)
                .GoWithHole(new Vector3(0,-1,3).normalized, 0.3f, 0.3f, true )
                .Build();



            foreach (var surface in surfaces)
            {
                var curveBlock = BlockFromPrefab(_curveBlock);
                curveBlock.Initialize(surface, _pathMaterial);
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
