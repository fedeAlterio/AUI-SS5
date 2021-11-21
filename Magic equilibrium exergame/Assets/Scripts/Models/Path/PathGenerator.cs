using Assets.Scripts.Models.Path.Blocks;
using Assets.Scripts.Models.Path.Blocks.Line;
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
        }



        // Public
        public void GenerateLine()
        {
            var line = LineBuilder.NewLine(Vector3.zero, Vector3.forward, 1)
                .MoveOf(Vector3.forward * 10)
                .MoveOf(Vector3.right * 10)
                .MoveOf(Vector3.right * 10)
                .MoveOf(Vector3.forward * 10)
                .MoveOf(new Vector3(0,1,3).normalized * 10)     
                .MoveOf(new Vector3(0,-1,3).normalized * 10)
                //.MoveOf(Vector3.right * 10)
                //.MoveOf(new Vector3(0, 1, 1))
                //.MoveOf(Vector3.right )
                //.MoveOf()
                //.MoveOf(Vector3.forward * 10)
                .Build();

            BlockFromPrefab(_curveBlock).Initialize(line);
        }


        // utils
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
