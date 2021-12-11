using Assets.Scripts.GameDebug;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Sea
{
    public class OceanManager : MonoBehaviour
    {
        // Editor fields
        [SerializeField] private GameObject _seaBlockPrefab;
        [SerializeField] private int _rows;
        [SerializeField] private int _columns;
        [SerializeField] private List<GameObject> _seaBlocks = new List<GameObject>();
        [SerializeField] private int _closePlayerVertexCount;
        [SerializeField] private int _farPlayerVertexCount;
        [SerializeField] private int _tileSize;



        // Initialization
        private void Start()
        {
            BuildOcean();
        }



        // Public
        public void BuildOcean()
        {
            foreach (var sea in _seaBlocks)
                Destroy(sea);

            SeaBlock.InitializeBlocks(_farPlayerVertexCount, _closePlayerVertexCount);
            var seaScale = _seaBlockPrefab.transform.localScale;
            for(var i = 0; i < _rows; i++)
                for(var j = 0; j < _columns; j++)
                {
                    var sea = Instantiate(_seaBlockPrefab);
                    _seaBlocks.Add(sea.gameObject);
                    sea.transform.parent = transform;
                    sea.transform.localPosition = new Vector3(j * _tileSize, transform.position.y, i * _tileSize);
                }
            transform.position = new Vector3(-_columns * _tileSize/2, transform.position.y, transform.position.z);
        }
    }
}
