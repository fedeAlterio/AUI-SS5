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
        [SerializeField] private List<PathBlock> _path = new List<PathBlock>();



        // Properties
        public IReadOnlyList<PathBlock> Path => _path;
        public PathBlock LastBlock => _path.Count > 0 ? _path[_path.Count - 1] : null;



        // Public Methods
        public void AddBlock(PathBlock pathBlock)
        {                                    
            pathBlock.transform.parent = _pathTransform;

            var lastBlock = LastBlock;
            if(lastBlock == null)
                pathBlock.transform.position = Vector3.zero;
            else
            {
                var rotation = Quaternion.FromToRotation(pathBlock.EntryDirection, lastBlock.ExitDirection);
                pathBlock.transform.localRotation *= rotation;
                pathBlock.transform.position = lastBlock.ExitPosition + (pathBlock.Position - pathBlock.EntryPosition);
            }            

            _path.Add(pathBlock);
        }

        public void RemoveBlock(PathBlock pathBlock)
        {            
            _path.Remove(pathBlock);
            Destroy(pathBlock.gameObject);
        }
    }
}
