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
        [SerializeField] private List<PathBlock> _blocksPrefabs = new List<PathBlock>();



        // Public 
        public void AddRandomBlock()
        {
            if (_blocksPrefabs.Count == 0)
                return;

            var blockPrefab = _blocksPrefabs.First();
            var block = BlockFromPrefab(blockPrefab);
            _pathManager.AddBlock(block);
        }

        public void RemoveLastBlock()
        {
            var lastBlock = _pathManager.LastBlock;
            if (lastBlock == null)
                return;

            _pathManager.RemoveBlock(lastBlock);
        }



        // Utils
        private PathBlock BlockFromPrefab(PathBlock pathBlock)
        {
            var ret = Instantiate(pathBlock);
            ret.name = pathBlock.BlockName;
            return ret;
        }
    }
}
