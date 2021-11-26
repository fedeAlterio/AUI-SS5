using Assets.Scripts.Models.Path.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Models.Path
{
    public class PathManager : MonoBehaviour
    {
        // Editor fields
        [SerializeField] private Transform _pathTransform;
        [SerializeField] private List<BaseBlock> _blocks = new List<BaseBlock>();



        // Properties
        public IReadOnlyList<BaseBlock> Blocks => _blocks;
        public BaseBlock LastBlock => _blocks.Count > 0 ? _blocks[_blocks.Count - 1] : null;



        // Public Methods
        public void Add(BaseBlock pathBlock, bool autoRotation = true)
        {                                    
            pathBlock.transform.parent = _pathTransform;

            var lastBlock = LastBlock;
            if(lastBlock == null)
                pathBlock.transform.position = Vector3.zero;
            else if(autoRotation)
            {
                var rotation = Quaternion.FromToRotation(pathBlock.EntryDirection, lastBlock.ExitDirection);
                pathBlock.transform.localRotation *= rotation;
                pathBlock.transform.position = lastBlock.ExitPosition + (pathBlock.Position - pathBlock.EntryPosition);
            }            

            _blocks.Add(pathBlock);
        }

        public void Remove(BaseBlock pathBlock)
        {            
            _blocks.Remove(pathBlock);
            Destroy(pathBlock.gameObject);
        }

        public void Clear()
        {
            foreach (var block in _blocks.ToList())
                Remove(block);
        }
    }
}
