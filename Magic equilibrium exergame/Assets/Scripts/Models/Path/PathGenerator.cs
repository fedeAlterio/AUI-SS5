using Assets.Scripts.Models.Path.Blocks;
using Assets.Scripts.Models.Path.Generation;
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



        // Initialization
        private void Start()
        {
            AddRandomBlock();
        }



        // Properties
        private Vector3 Up { get; } = Vector3.up;



        // Public 
        public void CurveRight() => CurveTo(Vector3.right);
        public void CurveLeft() => CurveTo(Vector3.left);
        public void CurveUp() => CurveTo(new Vector3(0, 0.3f, 1));
        public void CurveDown() => CurveTo(new Vector3(0, -0.3f, 1));
        public void Forward() => CurveTo(Vector3.forward);

        public void AddRandomBlock()
        {
            if (_blocksPrefabs.Count == 0)
                return;


            var blockPrefab = _blocksPrefabs.First();
            if (blockPrefab == null)
                return;

            var block = BlockFromPrefab(blockPrefab);
            _pathManager.AddBlock(block);
        }

        public void RemoveLastBlock()
        {
            if (_pathManager.Blocks.Count <= 1)
                return;

            var lastBlock = _pathManager.LastBlock;
            _pathManager.RemoveBlock(lastBlock);
        }

        public PathGenerator CurveTo(Vector3 direction)
        {
            var lastBlock = _pathManager.LastBlock;
            
            direction = direction.normalized;
            var z = lastBlock.ExitDirection.normalized;
            var x = Vector3.Cross(Up, z);
            var y = Vector3.Cross(z, x);

            var middle = lastBlock.ExitPosition + lastBlock.ExitDirection * 10;

            direction = direction.x * x + direction.y * y + direction.z * z;
            var end = middle + direction.normalized * 10;

            var curve = new QuadraticBezier(lastBlock.ExitPosition, middle, end);
            var curveBlock = BlockFromPrefab(_curveBlock);
            curveBlock.Initialize(curve);
            _pathManager.AddBlock(curveBlock, autoRotation: false);

            return this;
        }



        // Utils
        private T BlockFromPrefab<T>(T pathBlock) where T : BaseBlock
        {
            if (pathBlock == null)
                return default;

            var ret = Instantiate(pathBlock);
            //ret.name = pathBlock.BlockName;
            return ret;
        }
    }
}
